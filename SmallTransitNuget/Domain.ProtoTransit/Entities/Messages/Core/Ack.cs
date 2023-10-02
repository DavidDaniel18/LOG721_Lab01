namespace Domain.ProtoTransit.Entities.Messages.Core;

public sealed class Ack : ProtoTransit
{
    private Ack() : base(MessageTypesEnum.Ack) { }
}