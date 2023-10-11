using Interfaces;
using Interfaces.Cache;
using System.Collections.Concurrent;

namespace Entities.Cache
{
    public class BrokerCache : IBrokerCache
    {
        private IMemoryCache<ConcurrentDictionary<string, IBroker>> _brokers = new InMemoryCache<ConcurrentDictionary<string, IBroker>>(new ConcurrentDictionary<string, IBroker>());
        public IMemoryCache<ConcurrentDictionary<string, IBroker>> Brokers => _brokers;
    }
}
