using Interfaces.Cache;
using Interfaces.Domain;
using Interfaces.Repositories;
using System.Collections.Concurrent;

namespace Controllers.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        public ConcurrentDictionary<string, ISubscription>? Subscriptions => _cache.Subscriptions.GetValue();
        private readonly ISubscriptionCache _cache;
        public SubscriptionRepository(ISubscriptionCache cache) 
        {
            _cache = cache;
        }
    }
}
