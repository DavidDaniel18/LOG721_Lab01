using SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

namespace SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

public record PayloadType() : ProtoProperty(typeof(PayloadTypeItem));