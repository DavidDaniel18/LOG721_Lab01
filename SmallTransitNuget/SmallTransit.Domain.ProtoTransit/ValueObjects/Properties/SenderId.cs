using SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

namespace SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

public record SenderId() : ProtoProperty(typeof(SenderIdItem));