using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Interfaces.Domain;

namespace Interfaces.Repositories
{
    public interface ISubscriptionRepository
    {
        ConcurrentDictionary<Guid, ISubscription>? Subscriptions { get; }
    }
}
