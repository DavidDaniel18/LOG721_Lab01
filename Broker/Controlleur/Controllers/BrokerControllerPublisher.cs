using Entities;
using Interfaces.Domain;
using Interfaces.Handler;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Interfaces;

public sealed class BrokerControllerPublisher : IConsumer<BrokerReceiveWrapper>
{
    private readonly IPublisherHandler _publisherHandler;
    private readonly ILogger<BrokerControllerPublisher> _logger;

    public BrokerControllerPublisher(IPublisherHandler publisherHandler, ILogger<BrokerControllerPublisher> logger)
    {
        _publisherHandler = publisherHandler;
        _logger = logger;
    }

    public Task Consume(BrokerReceiveWrapper contract)
    {
        _logger.LogInformation($"Publication attempt for routing key {contract.RoutingKey}");

        IPublication publication = Publication.From(contract);
        _publisherHandler.Advertise(publication.RoutingKey);
        _publisherHandler.Publish(publication);

        return Task.CompletedTask;
    }
}