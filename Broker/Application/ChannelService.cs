using Interfaces;
using Interfaces.Domain;
using Interfaces.Repositories;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Application
{
    public class ChannelService : IChannelService
    {
        private readonly IRouter _router;
        private readonly IChannelRepository _channelRepository;

        public ChannelService(IChannelRepository channelRepository, IRouter router)
        {
            _channelRepository = channelRepository;
            _router = router;
        }

        public void AddChannel(string route)
        {
            _router.AddTopic(route);
            _ = _channelRepository.Channels?.TryAdd(route, Channel.CreateUnbounded<IPublication>());
        }

        public void AddMessage(string route, IPublication publication)
        {
            Channel<IPublication>? channel = null;
            _channelRepository.Channels?.TryGetValue(route, out channel);
            
            if (channel == null)
                return;

            while (!channel.Writer.TryWrite(publication)) 
            { 
                Thread.Sleep(500);
            };
        }

        public void RemoveChannel(string route)
        {
            _router.RemoveTopic(route);
            _ = _channelRepository.Channels?.TryRemove(route, out _);
        }
    }
}
