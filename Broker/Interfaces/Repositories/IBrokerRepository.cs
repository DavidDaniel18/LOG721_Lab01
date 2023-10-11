using System.Collections.Concurrent;

namespace Interfaces.Repositories
{
    public interface IBrokerRepository
    {
        ConcurrentDictionary<string, IBroker>? Brokers { get; }
    }
}
