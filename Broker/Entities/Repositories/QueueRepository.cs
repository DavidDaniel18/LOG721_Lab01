using Interfaces.Cache;
using Interfaces.Domain;
using Interfaces.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Controllers.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        public ConcurrentDictionary<string, Channel<IPublication>>? Queues => _cache.Queues.GetValue();
        private IQueueCache _cache;
        public QueueRepository(IQueueCache cache)
        {
            _cache = cache;
        }
    }
}
