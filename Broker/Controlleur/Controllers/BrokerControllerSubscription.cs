
using Application;
using Entities;
using Interfaces.Domain;
using Interfaces.Handler;
using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Interfaces;

public sealed class BrokerControllerSubscription : IConsumer<BrokerSubscriptionDto>
{
    private readonly ISubscriptionHandler _subscriptionHandler;

    public BrokerControllerSubscription(ISubscriptionHandler subscriptionHandler)
    {
        _subscriptionHandler = subscriptionHandler;
    }

    public async Task Consume(BrokerSubscriptionDto contract)
    {
        await Task.Run(() =>
        {
            ISubscription subscription = Subscription.From(contract);
            _subscriptionHandler.Subscribe(subscription);
            _subscriptionHandler.Listen(subscription);
        });
    }
}