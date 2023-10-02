using Domain.ProtoTransit.Entities.Header;

namespace Domain.ProtoTransit.Entities.Messages.Core.NegativeAcknowledge;

public sealed class Nack : ProtoTransit
{
    private Nack() : base(new ProtoHeader(MessageTypesEnum.Nack))
    {
    }
}