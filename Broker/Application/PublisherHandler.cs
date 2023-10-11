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
            _router.AddTopic(routingKey);
        }

        public void Publish(IPublication publication)
        {
            List<string> queues = _queueService.GetQueues(publication.RoutingKey);
            foreach (var queue in queues)
                _queueService.AddPublication(queue, publication);
        }

        public void UnAdvertise(string routingKey)
        {
            _router.RemoveTopic(routingKey);
        }
    }
}
