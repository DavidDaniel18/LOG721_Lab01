namespace Domain.Services.Send.Subscribing.Dto;

internal record SubscriptionDto(byte[] RoutingKey, byte[] PayloadType, byte[] QueueName);