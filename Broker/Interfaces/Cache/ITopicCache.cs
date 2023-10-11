using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Domain;

namespace Interfaces.Cache
{
    public interface ITopicCache
    {
        IMemoryCache<ITopicNode> Root { get; }
    }
}
