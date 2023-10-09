using Interfaces;
using Interfaces.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class QueueService : IQueueService
    {
        private readonly IRouter _router;
        private readonly IMessageService _messageService;
        private readonly IQueueRepository _queueRepository;

        public QueueService(IRouter router, IMessageService messageService, IQueueRepository queueRepository)
        {
            _router = router;
            _messageService = messageService;
            _queueRepository = queueRepository;
        }

        public bool AddQueue(string route, Format format)
        {
            return _queueRepository.Queues?.TryAdd(route, new ConcurrentQueue<IPublication>()) ?? false;
        }

        public bool Publish(IPublication publication)
        {
            var routesEnumerable = _router.GetTopics(publication.RoutingKey);
            var routes = routesEnumerable.ToList();
            foreach (var route in routes)
                _messageService.AddMessageToQueue(route, publication);
            return routes.Any();
        }

        public bool RemoveQueue(string route, Format format)
        {
            return _queueRepository.Queues?.TryRemove(route, out _) ?? false;
        }
    }
}
