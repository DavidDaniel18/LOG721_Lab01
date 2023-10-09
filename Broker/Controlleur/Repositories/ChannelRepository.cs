using Interfaces;
using Interfaces.Cache;
using Interfaces.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Controllers.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        public ConcurrentDictionary<string, Channel<IPublication>>? Channels => _cache.Channels.GetValue();
        private IChannelCache _cache;
        public ChannelRepository(IChannelCache cache)
        {
            _cache = cache;
        }
    }
}
