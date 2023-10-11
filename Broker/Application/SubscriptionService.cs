using Interfaces.Domain;
using Interfaces.Repositories;
using Interfaces.Services;
using System.Threading.Channels;

namespace Application
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IQueueRepository _queueRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IQueueRepository queueRepository) 
        {
            _subscriptionRepository = subscriptionRepository;
            _queueRepository = queueRepository;
        }

        public void AddSubscription(ISubscription subscription)
        {
            _subscriptionRepository.Subscriptions?.TryAdd(subscription.QueueName, subscription);
            _queueRepository.Queues?.TryAdd(subscription.QueueName, Channel.CreateUnbounded<IPublication>());
        }

        public void RemoveSubscription(ISubscription subscription)
        {
            _subscriptionRepository.Subscriptions?.TryRemove(subscription.QueueName, out _);
            _queueRepository.Queues?.TryRemove(subscription.QueueName, out _);
        }
    }
}
