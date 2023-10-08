namespace Domain.ProtoTransit.Entities.Messages.Core;

internal sealed class HandShake : Protocol
{
    private HandShake() : base(MessageTypesEnum.HandShake)
    {

    }
}