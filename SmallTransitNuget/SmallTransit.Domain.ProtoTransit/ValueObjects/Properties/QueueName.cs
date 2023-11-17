using SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

namespace SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

public record QueueName() : ProtoProperty(typeof(QueueNameItem));