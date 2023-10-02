using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.ValueObjects.Properties;

internal record PayloadType() : ProtoProperty(typeof(PayloadTypeItem));