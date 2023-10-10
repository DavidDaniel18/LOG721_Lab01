using Application;
using Interfaces.Cache;
using Interfaces.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Cache
{
    public class TopicCache : ITopicCache
    {
        private IMemoryCache<ITopicNode> _root = new InMemoryCache<ITopicNode>(ConcurrentTopicNode.Create());
        public IMemoryCache<ITopicNode> Root => _root;
    }
}
