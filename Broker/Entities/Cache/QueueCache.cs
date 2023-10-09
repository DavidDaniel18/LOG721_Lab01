using Interfaces;
using Interfaces.Cache;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Cache
{
    public class QueueCache : IQueueCache
    {
        private IMemoryCache<ConcurrentDictionary<string, ConcurrentQueue<IPublication>>> _queues = new InMemoryCache<ConcurrentDictionary<string, ConcurrentQueue<IPublication>>>(new ConcurrentDictionary<string, ConcurrentQueue<IPublication>>());
        public IMemoryCache<ConcurrentDictionary<string, ConcurrentQueue<IPublication>>> Queues => _queues;
    }
}
