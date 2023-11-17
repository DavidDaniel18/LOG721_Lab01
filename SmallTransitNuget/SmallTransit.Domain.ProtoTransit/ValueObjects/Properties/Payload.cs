using SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

namespace SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

public record Payload() : ProtoProperty(typeof(PayloadSizeItem));