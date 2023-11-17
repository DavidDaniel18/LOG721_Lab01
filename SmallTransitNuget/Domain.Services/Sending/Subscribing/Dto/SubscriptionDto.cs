namespace SmallTransit.Domain.Services.Sending.Subscribing.Dto;

internal record SubscriptionDto(byte[] RoutingKey, byte[] PayloadType, byte[] QueueName);