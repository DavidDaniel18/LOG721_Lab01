using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class PublisherHandler : IPublisherHandler
    {
        private readonly IQueueService _queueService;

        public PublisherHandler(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public bool Advertise(string topic, Format format)
        {
            return _queueService.AddQueue(topic, format);
        }

        public bool Publish(IPublication publication)
        {
            return _queueService.Publish(publication);
        }

        public bool UnAdvertise(string topic, Format format)
        {
            return _queueService.RemoveQueue(topic, format);
        }
    }
}
