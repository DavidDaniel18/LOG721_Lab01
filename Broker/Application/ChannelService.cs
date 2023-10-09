using Interfaces;
using Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Application
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;

        public ChannelService(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }

        public void AddChannel(ISubscription subscription)
        {
            
        }

        public void AddMessage(string queueName, IPublication publication)
        {
            Channel<IPublication>? channel = null;
            _channelRepository.Channels?.TryGetValue(queueName, out channel);
            
            if (channel == null)
                return;

            while (!channel.Writer.TryWrite(publication)) 
            { 
                Thread.Sleep(500);
            };
        }

        public void RemoveChannel(ISubscription subscription)
        {
            while (!(_channelRepository.Channels?.TryRemove(subscription.Topic, out _) ?? false))
            {
                Thread.Sleep(500);
            }
        }
    }
}
