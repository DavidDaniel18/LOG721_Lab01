using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.ValueObjects.Properties;

public record Payload() : ProtoProperty(typeof(PayloadSizeItem));