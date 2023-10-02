namespace Domain.ProtoTransit.Entities.Messages.Core;

public sealed class Nack : ProtoTransit
{
    private Nack() : base(MessageTypesEnum.Nack)
    {
    }
}