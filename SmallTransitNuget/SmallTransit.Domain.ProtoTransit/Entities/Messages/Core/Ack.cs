namespace SmallTransit.Domain.ProtoTransit.Entities.Messages.Core;

internal sealed class Ack : Protocol
{
    public Ack() : base(MessageTypesEnum.Ack) { }
}