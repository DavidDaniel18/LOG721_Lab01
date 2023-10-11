using System.Collections.Concurrent;
using Interfaces.Domain;

namespace Interfaces.Cache
{
    public interface ISubscriptionCache
    {
        IMemoryCache<ConcurrentDictionary<string, ISubscription>> Subscriptions { get; }
    }
}
