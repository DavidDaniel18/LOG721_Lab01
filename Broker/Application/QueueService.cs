using Interfaces;
using Interfaces.Domain;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Application
{
    public class QueueService : IQueueService
    {
        private readonly IQueueRepository _queueRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IRouter _router;
        private readonly ILogger<QueueService> _logger;

        public QueueService(ILogger<QueueService> logger, IQueueRepository queueRepository, ISubscriptionRepository subscriptionRepository, IRouter router) 
        {
            _subscriptionRepository = subscriptionRepository;
            _queueRepository = queueRepository;
            _router = router;
            _logger = logger;
        }

        public void AddPublication(string queueName, IPublication publication)
        {
            ISubscription? subscription = null;
            _subscriptionRepository.Subscriptions?.TryGetValue(queueName, out subscription);

            if (subscription == null) return;

            // todo: uncomment this section when mismatch fixed.
            //if (!string.Equals(subscription.Type, publication.Contract))
            //{
            //    _logger.LogInformation($"Contract mismatch: '{subscription.Type}' != '{publication.Contract}'");
            //    return;
            //}

            Channel<IPublication>? queue = null;
            _queueRepository.Queues?.TryGetValue(queueName, out queue);

            if (queue == null) return;

            AddPublication(queue, publication);
        }

        public void AddPublication(Channel<IPublication> queue, IPublication publication)
        {
            queue.Writer?.TryWrite(publication);
        }

        public List<string> GetQueues(string routingKey)
        {
            return _subscriptionRepository.Subscriptions?.Where(entry =>
            {
                IEnumerable<string> topics = _router.GetTopics(entry.Value.RoutingKey);
                return topics.Contains(routingKey);
            }).Select(x => x.Key).ToList() ?? new List<string>();
        }
    }
}
