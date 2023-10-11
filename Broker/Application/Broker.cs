using Domain.Common;
using Entities;
using Interfaces;
using Interfaces.Domain;
using Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Application
{
    public class Broker : IBroker
    {
        private readonly IQueueRepository _queueRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ISubscription _subscription;
        private readonly ILogger<BrokerService> _logger;

        public Broker(IQueueRepository queueRepository, ILogger<BrokerService> logger, ISubscriptionRepository subscriptionRepository, ISubscription subscription) 
        { 
            _queueRepository = queueRepository;
            _subscriptionRepository = subscriptionRepository;
            _subscription = subscription;
            _logger = logger;
        }

        public void Listen()
        {
            ISubscription? subscription = null;
            _ = _subscriptionRepository.Subscriptions?.TryGetValue(_subscription.QueueName, out subscription);

            if (subscription == null) return; // No more subscriptions return.

            _logger.LogInformation($"subscription exists for {subscription.QueueName}");

            Channel<IPublication>? channel = null;
            _queueRepository.Queues?.TryGetValue(subscription.QueueName, out channel);

            if (channel == null) return; // The channel does not exists.

            _logger.LogInformation($"Channel exists for {subscription.QueueName}");

            IBrokerChannelListener listener = new BrokerChannelListener(_logger, channel, subscription.Endpoint);

            _ = ListenChannelAsync(subscription, listener);
        }

        private async Task ListenChannelAsync(ISubscription subscription, IBrokerChannelListener listener)
        {
            _logger.LogInformation($"Listen {subscription.QueueName}");

            var result = await listener.Listen();

            if (result.IsFailure())
            {
                _logger.LogInformation($"Listen Failure {subscription.QueueName}");
            }

            _logger.LogInformation($"Listen End {subscription.QueueName}");
        }
    }
}
