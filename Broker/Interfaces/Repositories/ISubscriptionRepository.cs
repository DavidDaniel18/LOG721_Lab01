using System.Collections.Concurrent;
using Interfaces.Domain;

namespace Interfaces.Repositories
{
    public interface ISubscriptionRepository
    {
        ConcurrentDictionary<string, ISubscription>? Subscriptions { get; }
    }
}
