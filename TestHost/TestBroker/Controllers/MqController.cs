using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Interfaces;

namespace TestBroker.Controllers;

public sealed class MqController : IConsumer<BrokerSubscriptionDto>
{
    private readonly ILogger<MqController> _logger;
    public static IBrokerPushEndpoint Endpoint;

    public MqController(ILogger<MqController> logger)
    {
        _logger = logger;
    }

    public Task Consume(BrokerSubscriptionDto contract)
    {
        _logger.LogInformation("Received: {0} subscription", contract.RoutingKey);

        Endpoint = contract.BrokerPushEndpoint;

        return Task.CompletedTask;
    }
}