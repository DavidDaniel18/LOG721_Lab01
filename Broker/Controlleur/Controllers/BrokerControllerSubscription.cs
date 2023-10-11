
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

    public Task Consume(BrokerSubscriptionDto contract)
    {
        ISubscription subscription = Subscription.From(contract);
        // creates subscription
        // creates queue
        _subscriptionHandler.Subscribe(subscription);
        // creates listener broker and adds it to the broker service.
        _subscriptionHandler.Listen(subscription);

        return Task.CompletedTask;
    }
}