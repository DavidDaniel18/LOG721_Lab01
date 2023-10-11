using SmallTransit.Abstractions.Interfaces;

namespace Interfaces.Domain
{
    public interface ISubscription
    {
        string RoutingKey { get; }
        string QueueName { get; }
        string Type { get; }
        IBrokerPushEndpoint Endpoint { get; }
    }
}
