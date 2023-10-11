using System.Collections.Concurrent;
using System.Threading.Channels;
using Interfaces.Domain;

namespace Interfaces.Cache
{
    public interface IQueueCache
    {
        IMemoryCache<ConcurrentDictionary<string, Channel<IPublication>>> Queues { get; }
    }
}
