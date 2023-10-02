using Domain.ProtoTransit.Entities.Header;

namespace Domain.ProtoTransit.Entities.Messages.Core.HandShake;

public sealed class HandShake : ProtoTransit
{
    private HandShake() : base(new ProtoHeader(MessageTypesEnum.HandShake))
    {

    }
}