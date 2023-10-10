using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Interfaces.Domain;

namespace Interfaces.Cache
{
    public interface IChannelCache
    {
        IMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>> Channels { get; }
    }
}
