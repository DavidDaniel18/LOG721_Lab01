using Interfaces;
using Interfaces.Cache;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Cache
{
    public class MessageCache : IMessageCache
    {
        private InMemoryCache<ConcurrentQueue<IPublication>> _messagesQueue = new InMemoryCache<ConcurrentQueue<IPublication>>(new ConcurrentQueue<IPublication>());
        public IMemoryCache<ConcurrentQueue<IPublication>> MessagesQueue => _messagesQueue;
    }
}
