using Interfaces.Domain;
using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Interfaces;

namespace Entities
{
    public class Subscription : ISubscription
    {
        public IBrokerPushEndpoint Endpoint { get; }
        public string QueueName { get; }
        public string RoutingKey { get; }
        public string Type { get;}

        public Subscription(IBrokerPushEndpoint endpoint, string queueName, string routingKey, string type)
        {
            Endpoint = endpoint;
            QueueName = queueName;
            RoutingKey = routingKey;
            Type = type;
        }

        public static ISubscription From(BrokerSubscriptionDto dto)
        {
            return new Subscription(dto.BrokerPushEndpoint, dto.QueueName, dto.RoutingKey, dto.PayloadType);
        }
    }
}
