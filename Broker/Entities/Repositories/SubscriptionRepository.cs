using Entities;
using Interfaces.Cache;
using Interfaces.Domain;
using Interfaces.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        public ConcurrentDictionary<Guid, ISubscription>? Subscriptions => _cache.Subscriptions.GetValue();
        private readonly ISubscriptionCache _cache;
        public SubscriptionRepository(ISubscriptionCache cache) 
        {
            _cache = cache;
        }
    }
}
