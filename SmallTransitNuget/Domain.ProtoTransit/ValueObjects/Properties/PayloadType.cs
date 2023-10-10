using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.ValueObjects.Properties;

public record PayloadType() : ProtoProperty(typeof(PayloadTypeItem));