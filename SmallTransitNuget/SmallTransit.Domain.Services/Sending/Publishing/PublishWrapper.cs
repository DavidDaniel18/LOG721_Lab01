namespace SmallTransit.Domain.Services.Sending.Publishing;

public record PublishWrapper<TContract>(TContract Payload, string RoutingKey);