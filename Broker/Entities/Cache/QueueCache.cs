using Interfaces.Cache;
using Interfaces.Domain;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Entities.Cache
{
    public class QueueCache : IQueueCache
    {
        private IMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>> _queues = new InMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>>(new ConcurrentDictionary<string, Channel<IPublication>>());
        public IMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>> Queues => _queues;
    }
}
