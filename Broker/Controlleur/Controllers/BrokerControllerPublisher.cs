

using Entities;
using Interfaces.Domain;
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
        IPublication publication = Publication.From(contract);
        // Add the topic to the router.
        _publisherHandler.Advertise(publication.RoutingKey);
        // Add the publication to all queues that match the routing key with their pattern.
        _publisherHandler.Publish(publication);

        return Task.CompletedTask;
    }
}