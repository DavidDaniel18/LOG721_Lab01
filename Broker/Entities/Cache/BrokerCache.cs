using Interfaces;
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
    public class BrokerCache : IBrokerCache
    {
        IMemoryCache<ConcurrentDictionary<Guid, IBroker>> _brokers = new InMemoryCache<ConcurrentDictionary<Guid, IBroker>>(new ConcurrentDictionary<Guid, IBroker>());
        public IMemoryCache<ConcurrentDictionary<Guid, IBroker>> Brokers => _brokers;
    }
}
