using Application;
using Interfaces.Cache;
using Interfaces.Domain;

namespace Entities.Cache
{
    public class TopicCache : ITopicCache
    {
        private IMemoryCache<ITopicNode> _root = new InMemoryCache<ITopicNode>(ConcurrentTopicNode.Create());
        public IMemoryCache<ITopicNode> Root => _root;
    }
}
