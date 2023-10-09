using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Cache
{
    public interface IMessageCache
    {
        IMemoryCache<ConcurrentQueue<IPublication>> MessagesQueue { get; } 
    }
}
