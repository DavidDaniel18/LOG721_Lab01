using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.ValueObjects.Properties;

internal record RoutingKey() : ProtoProperty(typeof(RoutingKeyItem));