using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Interfaces.Cache;

namespace Application
{
    public class Router : IRouter
    {
        private readonly ITopicCache _cache;
        private ITopicNode? _root => _cache.Root?.GetValue();

        public Router(ITopicCache cache)
        {
            _cache = cache;
        }

        public void AddTopic(string pattern)
        {
            _root?.AddTopicNodes(pattern);
        }

        public IEnumerable<string> GetTopics(string pattern)
        {
            return _root?.GetTopicNodes(pattern).Select(x => x.Key).ToList() ?? new List<string>();
        }

        public void InitializeTopicNodes(List<string> allRoutePatterns)
        {
            foreach (var pattern in allRoutePatterns)
                _root?.AddTopicNodes(pattern);
        }

        public void RemoveTopic(string pattern)
        {
            _root?.RemoveTopicNodes(pattern);
        }
    }
}
