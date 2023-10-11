using Interfaces.Cache;
using Interfaces.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IQueueRepository
    {
        ConcurrentDictionary<string, Channel<IPublication>>? Queues { get; }
    }
}
