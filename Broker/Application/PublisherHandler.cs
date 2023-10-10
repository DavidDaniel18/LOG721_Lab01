using Interfaces.Domain;
using Interfaces.Handler;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class PublisherHandler : IPublisherHandler
    {
        private readonly IChannelService _channelService;

        public PublisherHandler(IChannelService channelService)
        {
            _channelService = channelService;
        }

        public void Advertise(string route)
        {
            _channelService.AddChannel(route);
        }

        public void Publish(IPublication publication)
        {
            _channelService.AddMessage(publication.RoutingKey, publication);
        }

        public void UnAdvertise(string route)
        {
            _channelService.RemoveChannel(route);
        }
    }
}
