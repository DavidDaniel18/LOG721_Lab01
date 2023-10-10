using Application.Common.Interfaces;

namespace Application.Common.Broker;

public sealed record BrokerSubscriptionDto(string RoutingKey, string PayloadType, string QueueName, IBrokerPushEndpoint BrokerPushEndpoint);