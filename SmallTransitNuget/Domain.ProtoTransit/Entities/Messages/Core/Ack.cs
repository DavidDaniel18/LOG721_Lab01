namespace Domain.ProtoTransit.Entities.Messages.Core;

internal sealed class Ack : Protocol
{
    private Ack() : base(MessageTypesEnum.Ack) { }
}