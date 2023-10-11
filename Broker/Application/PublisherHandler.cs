using Interfaces;
using Interfaces.Domain;
using Interfaces.Handler;
using Interfaces.Services;

namespace Application
{
    public class PublisherHandler : IPublisherHandler
    {
        private readonly IRouter _router;
        private readonly IQueueService _queueService;

        public PublisherHandler(IQueueService queueService, IRouter router)
        {
            _queueService = queueService;
            _router = router;
        }

        public void Advertise(string routingKey)
        {
            // Advertise routing key to the router. 
            _router.AddTopic(routingKey);
        }

        public void Publish(IPublication publication)
        {
            // Get all queues routing key pattern matches publication routing key.
            List<string> queues = _queueService.GetQueues(publication.RoutingKey);
            // Add publication to queue.
            foreach (var queue in queues)
                _queueService.AddPublication(queue, publication);
        }

        public void UnAdvertise(string routingKey)
        {
            // Un advertise routing key to the router
            _router.RemoveTopic(routingKey);
        }
    }
}
