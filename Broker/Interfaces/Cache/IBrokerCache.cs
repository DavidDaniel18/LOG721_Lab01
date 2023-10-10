using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Interfaces.Cache
{
    public interface IBrokerCache
    {
        IMemoryCache<ConcurrentDictionary<Guid, IBroker>> Brokers { get; }
    }
}
