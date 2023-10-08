namespace Domain.Services.Send.Subscribing.Dto;

public record SubscriptionWrapper(string RoutingKey, Type PayloadType, string QueueName);