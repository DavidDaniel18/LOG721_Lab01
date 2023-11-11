using Application.Common.Interfaces;
using SmallTransit.Abstractions.Interfaces;

namespace Infrastructure.Clients.Tcp;

public sealed class SmallTransitPublisher<TMessage> : IMessagePublisher<TMessage> where TMessage : class
{
    private readonly IPublisher<TMessage> _publisher;

    public SmallTransitPublisher(IPublisher<TMessage> publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishAsync(TMessage message, string routingKey)
    {
        await _publisher.Publish(message, routingKey);
    }
}