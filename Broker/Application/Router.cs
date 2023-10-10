using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Interfaces.Cache;
using Interfaces.Domain;

namespace Application
{
    public class Router : IRouter, IObservable
    {
        private readonly ITopicCache _cache;
        private ITopicNode? _root => _cache.Root?.GetValue();
        private List<IObserver> _observers = new List<IObserver>();
        public List<IObserver> Observers => _observers;

        public Router(ITopicCache cache)
        {
            _cache = cache;
        }

        public void AddTopic(string pattern)
        {
            _root?.AddTopicNodes(pattern);
            Notify();
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
            Notify();
        }

        public void Notify()
        {
            foreach (var observer in _observers)
                observer.OnUpdate();
        }

        public void AddObserver(IObserver observer)
        {
            _observers?.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers?.Remove(observer);
        }
    }
}
