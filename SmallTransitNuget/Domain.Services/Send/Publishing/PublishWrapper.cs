namespace Domain.Services.Send.Publishing;

public record PublishWrapper(object payload, string routingKey);