using Entities;
using Interfaces;
using Interfaces.Domain;
using Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Application
{
    public class Broker : IBroker, IObserver
    {
        private readonly IChannelRepository _channelRepository;

        private readonly ISubscriptionRepository _subscriptionRepository;
        
        private readonly IRouter _router;

        private readonly Guid _subscriptionGuid;

        private readonly string _routePattern;

        private Dictionary<string, IBrokerChannelListener> _brokerChannelListeners = new Dictionary<string, IBrokerChannelListener>();

        public Broker(IChannelRepository channelRepository, IRouter router, ISubscriptionRepository subscriptionRepository, string routePattern, Guid subscriptionId) 
        { 
            _channelRepository = channelRepository;
            _router = router;
            _routePattern = routePattern;
            _subscriptionRepository = subscriptionRepository;
            _subscriptionGuid = subscriptionId;
        }

        public void Listen()
        {
            ISubscription? subscription = null;
            _ = _subscriptionRepository.Subscriptions?.TryGetValue(_subscriptionGuid, out subscription);

            if (subscription == null) return; // No more subscriptions return.

            foreach (var route in _router.GetTopics(_routePattern))
            {
                bool isAlreadyAdded = _brokerChannelListeners.ContainsKey(route);
                
                if (isAlreadyAdded) continue; // No need to add listener.

                Channel<IPublication>? channel = null;
                _channelRepository.Channels?.TryGetValue(route, out channel);

                if (channel == null) continue; // No channel, no need to add listener.

                IBrokerChannelListener listener = new BrokerChannelListener(channel, subscription.Endpoint);

                _brokerChannelListeners.Add(route, listener);

                Task.Run(() => listener.Listen());
            }
        }

        public void OnUpdate()
        {
            Listen();
        }
    }
}
