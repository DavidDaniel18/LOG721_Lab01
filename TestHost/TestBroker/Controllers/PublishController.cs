using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Interfaces;

namespace TestBroker.Controllers;

public sealed class PublishController : IConsumer<BrokerReceiveWrapper>
{
    private readonly ILogger<PublishController> _logger;

    public PublishController(ILogger<PublishController> logger)
    {
        _logger = logger;
    }

    public async Task Consume(BrokerReceiveWrapper contract)
    {
        _logger.LogInformation("Received: {0}, now pushing", contract.Payload);

        await MqController.Endpoint.Push(contract.Payload);
    }
}