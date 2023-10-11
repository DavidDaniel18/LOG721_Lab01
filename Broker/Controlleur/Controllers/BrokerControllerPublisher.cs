

using Entities;
using Interfaces.Handler;
using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Interfaces;

public sealed class BrokerControllerPublisher : IConsumer<BrokerReceiveWrapper>
{
    private readonly IPublisherHandler _publisherHandler;

    public BrokerControllerPublisher(IPublisherHandler publisherHandler) 
    { 
        _publisherHandler = publisherHandler;
    }

    public Task Consume(BrokerReceiveWrapper contract)
    {
        _publisherHandler.Advertise(contract.RoutingKey);
        _publisherHandler.Publish(new Publication(contract.Contract, contract.RoutingKey, contract.Payload));

        return Task.CompletedTask;
    }
}