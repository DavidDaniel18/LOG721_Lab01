using Domain.ProtoTransit.Entities.Header;

namespace Domain.ProtoTransit.Entities.Messages.Core.Acknowledge;

public sealed class Ack : ProtoTransit
{
    private Ack() : base(new ProtoHeader(MessageTypesEnum.Ack)) { }
}