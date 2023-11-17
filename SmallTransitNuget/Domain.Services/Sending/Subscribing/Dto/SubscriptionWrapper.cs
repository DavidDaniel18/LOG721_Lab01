namespace SmallTransit.Domain.Services.Sending.Subscribing.Dto;

public record SubscriptionWrapper(string RoutingKey, string PayloadType, string QueueName);