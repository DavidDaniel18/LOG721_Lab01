using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.ValueObjects.Properties;

internal record Payload() : ProtoProperty(typeof(PayloadSizeItem));