using Interfaces;
using Interfaces.Domain;
using Interfaces.Repositories;
using Interfaces.Services;
using System.Threading.Channels;

namespace Application
{
    public class QueueService : IQueueService
    {
        private readonly IQueueRepository _queueRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IRouter _router;

        public QueueService(IQueueRepository queueRepository, ISubscriptionRepository subscriptionRepository, IRouter router) 
        {
            _subscriptionRepository = subscriptionRepository;
            _queueRepository = queueRepository;
            _router = router;
        }

        public void AddPublication(string queueName, IPublication publication)
        {
            Channel<IPublication>? queue = null;
            _queueRepository.Queues?.TryGetValue(queueName, out queue);

            if (queue == null) return;

            AddPublication(queue, publication);
        }

        public void AddPublication(Channel<IPublication> queue, IPublication publication)
        {
            queue.Writer?.TryWrite(publication);
        }

        public string GetQueue(string routingKey)
        {
            return GetQueues(routingKey)?.First() ?? "";
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
