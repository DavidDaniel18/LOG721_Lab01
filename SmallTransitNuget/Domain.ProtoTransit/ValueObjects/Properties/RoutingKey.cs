using SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

namespace SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

public record RoutingKey() : ProtoProperty(typeof(RoutingKeyItem));