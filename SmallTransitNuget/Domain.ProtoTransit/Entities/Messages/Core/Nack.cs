namespace Domain.ProtoTransit.Entities.Messages.Core;

internal sealed class Nack : Protocol
{
    private Nack() : base(MessageTypesEnum.Nack)
    {
    }
}