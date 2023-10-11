namespace Domain.Services.Send.Subscribing.Dto;

public record SubscriptionWrapper(string RoutingKey, string PayloadType, string QueueName);