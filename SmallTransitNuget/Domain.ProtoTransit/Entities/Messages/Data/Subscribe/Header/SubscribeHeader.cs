using Domain.ProtoTransit.Entities.Header;

namespace Domain.ProtoTransit.Entities.Messages.Data.Subscribe.Header;

internal sealed partial class SubscribeHeader : ProtoHeader
{
    private const int RoutingKeyLength = 1;
    private const int PayloadTypeLength = 1;
    private const int QueueNameLength = 1;

    internal SubscribeHeader() : base(MessageTypesEnum.Subscribe) { }
}