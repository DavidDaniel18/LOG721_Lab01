using Interfaces.Domain;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Interfaces.Repositories
{
    public interface IQueueRepository
    {
        ConcurrentDictionary<string, Channel<IPublication>>? Queues { get; }
    }
}
