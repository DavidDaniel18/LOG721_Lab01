using Interfaces;
using Interfaces.Cache;
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
        private IMemoryCache<ConcurrentDictionary<string, ISubscription>> _subscriptions = new InMemoryCache<ConcurrentDictionary<string, ISubscription>>(new ConcurrentDictionary<string, ISubscription>());
        public IMemoryCache<ConcurrentDictionary<string, ISubscription>> Subscriptions => _subscriptions;
    }
}
