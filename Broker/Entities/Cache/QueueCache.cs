using Interfaces.Cache;
using Interfaces.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Entities.Cache
{
    public class QueueCache : IQueueCache
    {
        private IMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>> _queues = new InMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>>(new ConcurrentDictionary<string, Channel<IPublication>>());
        public IMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>> Queues => _queues;
    }
}
