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
    public class BrokerRepository : IBrokerRepository
    {
        public ConcurrentDictionary<Guid, IBroker>? Brokers => _cache.Brokers.GetValue();
        private IBrokerCache _cache;
        public BrokerRepository(IBrokerCache cache)
        {
            _cache = cache;
        }
    }
}
