using Interfaces;
using Interfaces.Domain;
using Interfaces.Repositories;
using Interfaces.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class BrokerService : IBrokerService
    {
        ConcurrentDictionary<string, IBroker>? brokers => _brokerRepository.Brokers;

        private readonly IChannelRepository _channelRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IBrokerRepository _brokerRepository;
        private readonly IRouter _router;

        public BrokerService(IBrokerRepository brokerRepository, IChannelRepository channelRepository, ISubscriptionRepository subscriptionRepository, IRouter router)
        {
            _channelRepository = channelRepository;
            _subscriptionRepository = subscriptionRepository;
            _router = router;
            _brokerRepository = brokerRepository;
        }

        public void AssignBroker(ISubscription subscription)
        {
            var broker = new Broker(_channelRepository, _router, _subscriptionRepository, subscription);

            if (!(brokers?.ContainsKey(subscription.QueueName) ?? false)) return; // Already contains broker.

            brokers.TryAdd(subscription.QueueName, broker);
            
            Task.Run(() => broker.Listen());
        }
    }
}
