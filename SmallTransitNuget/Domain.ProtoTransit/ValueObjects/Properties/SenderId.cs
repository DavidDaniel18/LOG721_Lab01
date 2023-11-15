using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.ValueObjects.Properties;

public record SenderId() : ProtoProperty(typeof(SenderIdItem));