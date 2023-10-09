using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Interfaces.Repositories
{
    public interface ISubscriptionRepository
    {
        ConcurrentDictionary<string, ISubscription>? Subscriptions { get; }
    }
}
