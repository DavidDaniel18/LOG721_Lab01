using System.Collections.Concurrent;

namespace Interfaces.Cache
{
    public interface IBrokerCache
    {
        IMemoryCache<ConcurrentDictionary<string, IBroker>> Brokers { get; }
    }
}
