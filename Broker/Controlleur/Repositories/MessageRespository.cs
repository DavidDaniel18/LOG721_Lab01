using Interfaces;
using Interfaces.Cache;
using Interfaces.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Repositories
{
    public class MessageRespository : IMessageRepository
    {
        public ConcurrentQueue<IPublication>? Messages => _cache.MessagesQueue.GetValue();
        private IMessageCache _cache;
        public MessageRespository(IMessageCache cache)
        {
            _cache = cache;
        }
    }
}
