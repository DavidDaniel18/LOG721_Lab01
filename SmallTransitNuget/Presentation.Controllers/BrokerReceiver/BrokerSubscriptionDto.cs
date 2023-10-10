using Presentation.Controllers.Intefaces;

namespace Presentation.Controllers.BrokerReceiver;

internal sealed record BrokerSubscriptionDto(string RoutingKey, string PayloadType, string QueueName, IBrokerPushEndpoint BrokerPushEndpoint);