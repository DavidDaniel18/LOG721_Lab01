using Interfaces;
using Interfaces.Cache;
using Interfaces.Repositories;
using System.Collections.Concurrent;

namespace Controllers.Repositories
{
    public class BrokerRepository : IBrokerRepository
    {
        public ConcurrentDictionary<string, IBroker>? Brokers => _cache.Brokers.GetValue();
        private IBrokerCache _cache;
        public BrokerRepository(IBrokerCache cache)
        {
            _cache = cache;
        }
    }
}
