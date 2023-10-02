namespace SmallTransit.Application.Common;

public record Payload<T>(string RoutingKey, T Message) where T : class;