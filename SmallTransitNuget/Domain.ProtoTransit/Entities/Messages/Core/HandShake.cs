namespace Domain.ProtoTransit.Entities.Messages.Core;

internal sealed class HandShake : Protocol
{
    public HandShake() : base(MessageTypesEnum.HandShake)
    {

    }
}