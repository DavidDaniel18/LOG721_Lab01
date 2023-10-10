namespace Domain.ProtoTransit.Entities.Messages.Core;

internal sealed class Nack : Protocol
{
    public Nack() : base(MessageTypesEnum.Nack)
    {
    }
}