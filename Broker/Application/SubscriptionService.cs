using Interfaces.Domain;
using Interfaces.Repositories;
using Interfaces.Services;

namespace Application
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IQueueRepository _queueRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository) 
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public void AddSubscription(ISubscription subscription)
        {
            _subscriptionRepository.Subscriptions?.TryAdd(subscription.QueueName, subscription);
        }

        public void RemoveSubscription(ISubscription subscription)
        {
            _subscriptionRepository.Subscriptions?.TryRemove(subscription.QueueName, out _);
        }
    }
}
