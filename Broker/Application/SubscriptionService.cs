using Interfaces.Domain;
using Interfaces.Repositories;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository) 
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public void AddSubscription(ISubscription subscription)
        {
            _subscriptionRepository.Subscriptions?.TryAdd(subscription.Id, subscription);
        }

        public void RemoveSubscription(ISubscription subscription)
        {
            _subscriptionRepository.Subscriptions?.TryRemove(subscription.Id, out _);
        }
    }
}
