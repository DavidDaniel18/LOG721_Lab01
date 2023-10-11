using Interfaces;
using Interfaces.Domain;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Application
{
    public class BrokerService : IBrokerService
    {
        ConcurrentDictionary<string, IBroker>? _brokers => _brokerRepository.Brokers;

        private readonly IQueueRepository _queueRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IBrokerRepository _brokerRepository;
        private readonly ILogger<BrokerService> _logger;

        public BrokerService(IQueueRepository queueRepository, ILogger<BrokerService> logger, IBrokerRepository brokerRepository, ISubscriptionRepository subscriptionRepository)
        {
            _queueRepository = queueRepository;
            _subscriptionRepository = subscriptionRepository;
            _brokerRepository = brokerRepository;
            _logger = logger;
        }

        public void AssignBroker(ISubscription subscription)
        {
            var broker = new Broker(_queueRepository, _logger, _subscriptionRepository, subscription);

            if (_brokers?.ContainsKey(subscription.QueueName) ?? true) return; // Already contains broker.

            _brokers.TryAdd(subscription.QueueName, broker);
            
            Task.Run(() => broker.Listen());
        }
    }
}
