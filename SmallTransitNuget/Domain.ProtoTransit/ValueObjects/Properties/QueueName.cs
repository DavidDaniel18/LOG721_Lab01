using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.ValueObjects.Properties;

internal record QueueName() : ProtoProperty(typeof(QueueNameItem));