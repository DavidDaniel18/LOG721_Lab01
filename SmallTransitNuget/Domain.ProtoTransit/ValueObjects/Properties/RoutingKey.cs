using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.ValueObjects.Properties;

public record RoutingKey() : ProtoProperty(typeof(RoutingKeyItem));