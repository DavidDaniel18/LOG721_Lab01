using Interfaces.Cache;
using Interfaces.Domain;
using System.Collections.Concurrent;

namespace Entities.Cache
{
    public class SubscriptionCache : ISubscriptionCache
    {
        private IMemoryCache<ConcurrentDictionary<string, ISubscription>> _subscriptions = new InMemoryCache<ConcurrentDictionary<string, ISubscription>>(new ConcurrentDictionary<string, ISubscription>());
        public IMemoryCache<ConcurrentDictionary<string, ISubscription>> Subscriptions => _subscriptions;
    }
}
