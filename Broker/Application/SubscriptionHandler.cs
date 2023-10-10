using Interfaces.Domain;
using Interfaces.Handler;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class SubscriptionHandler : ISubscriptionHandler
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IBrokerService _brokerService;

        public SubscriptionHandler(ISubscriptionService subscriptionService, IBrokerService brokerService) 
        { 
            _subscriptionService = subscriptionService;
            _brokerService = brokerService;
        } 

        public void Listen(ISubscription subscription)
        {
            _brokerService.AssignBroker(subscription);
        }

        public void Subscribe(ISubscription subscription)
        {
            _subscriptionService.AddSubscription(subscription);
        }

        public void Unsubscribe(ISubscription subscription)
        {
            _subscriptionService.RemoveSubscription(subscription);
        }
    }
}
