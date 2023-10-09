using Interfaces;
using Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Application
{
    public class Broker : IBroker
    {
        private IChannelRepository _channelRepository;

        public Broker(IChannelRepository channelRepository) 
        { 
            _channelRepository = channelRepository;
        }

        public void ListenToQueue(string queueName)
        {
            Channel<IPublication> channel = null;
            do
            {
                _channelRepository.Channels?.TryGetValue(queueName, out channel);
                if (channel != null)
                {
                    
                }
            } while (channel != null);
        }
    }
}
