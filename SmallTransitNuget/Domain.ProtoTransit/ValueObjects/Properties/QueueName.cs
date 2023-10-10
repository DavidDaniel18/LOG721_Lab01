using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.ValueObjects.Properties;

public record QueueName() : ProtoProperty(typeof(QueueNameItem));