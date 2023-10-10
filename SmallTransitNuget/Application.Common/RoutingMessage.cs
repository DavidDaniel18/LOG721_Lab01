namespace Application.Common;

public record RoutingMessage<T>(string RoutingKey, T Message);