using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class SubscriptionHandler : ISubsciptionHandler
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionHandler(ISubscriptionService subscriptionService) 
        { 
            _subscriptionService = subscriptionService;
        } 

        public void Litsen(ISubscription subscription)
        {
            // todo: not implemented
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
