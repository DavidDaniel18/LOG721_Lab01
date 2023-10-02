namespace Domain.ProtoTransit.Entities.Messages.Core;

public sealed class HandShake : ProtoTransit
{
    private HandShake() : base(MessageTypesEnum.HandShake)
    {

    }
}