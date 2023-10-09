﻿using Interfaces;
using Interfaces.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Entities.Cache
{
    public class ChannelCache : IChannelCache
    {
        IMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>> _channels = new InMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>>(new ConcurrentDictionary<string, Channel<IPublication>>());
        public IMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>> Channels => _channels;
    }
}
