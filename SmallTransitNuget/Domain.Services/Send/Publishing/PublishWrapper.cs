namespace Domain.Services.Send.Publishing;

public record PublishWrapper<TContract>(TContract Payload, string RoutingKey);