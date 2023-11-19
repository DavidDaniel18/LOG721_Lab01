using SmallTransit.Abstractions.Interfaces;

namespace SmallTransit.Abstractions.Broker;

public sealed record BrokerSubscriptionDto(string RoutingKey, string PayloadType, string QueueName, IBrokerPushEndpoint BrokerPushEndpoint);