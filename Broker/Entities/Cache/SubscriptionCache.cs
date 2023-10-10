using Interfaces.Cache;
using Interfaces.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Cache
{
    public class SubscriptionCache : ISubscriptionCache
    {
        private IMemoryCache<ConcurrentDictionary<Guid, ISubscription>> _subscriptions = new InMemoryCache<ConcurrentDictionary<Guid, ISubscription>>(new ConcurrentDictionary<Guid, ISubscription>());
        public IMemoryCache<ConcurrentDictionary<Guid, ISubscription>> Subscriptions => _subscriptions;
    }
}
