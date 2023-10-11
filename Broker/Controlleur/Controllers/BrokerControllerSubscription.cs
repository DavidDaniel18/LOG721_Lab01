using Entities;
using Interfaces.Domain;
using Interfaces.Handler;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Interfaces;

public sealed class BrokerControllerSubscription : IConsumer<BrokerSubscriptionDto>
{
    private readonly ISubscriptionHandler _subscriptionHandler;
    private readonly ILogger<BrokerControllerSubscription> _logger;

    public BrokerControllerSubscription(ISubscriptionHandler subscriptionHandler, ILogger<BrokerControllerSubscription> logger)
    {
        _subscriptionHandler = subscriptionHandler;
        _logger = logger;
    }

    public Task Consume(BrokerSubscriptionDto contract)
    {
        _logger.LogInformation($"Subscription attempt of queue {contract.QueueName}");

        ISubscription subscription = Subscription.From(contract);
        _subscriptionHandler.Subscribe(subscription);
        _subscriptionHandler.Listen(subscription);

        return Task.CompletedTask;
    }
}