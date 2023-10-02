using Domain.ProtoTransit.Entities.Header;

namespace Domain.ProtoTransit.Entities.Messages.Data.Subscribe.Header;

internal sealed partial class SubscribeHeader : ProtoHeader
{
    internal void AddHeaderLengthValues(uint routingKeyLengthValue, uint payloadTypeLengthValue, uint queueNameLengthValue)
    {
        AddHeaderItem(nameof(RoutingKeyLength), RoutingKeyLength, () => routingKeyLengthValue.ToBytes());
        AddHeaderItem(nameof(PayloadTypeLength), PayloadTypeLength, () => payloadTypeLengthValue.ToBytes());
        AddHeaderItem(nameof(QueueNameLength), QueueNameLength, () => queueNameLengthValue.ToBytes());
    }
}