using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IChannelService _channelService;

        public SubscriptionService(IChannelService channelService) 
        {
            _channelService = channelService;
        }

        public void AddSubscription(ISubscription subscription)
        {
            _channelService.AddChannel(subscription);
        }

        public void RemoveSubscription(ISubscription subscription)
        {
            _channelService.RemoveChannel(subscription);
        }
    }
}
