using Domain.ProtoTransit.Entities.Header;

namespace Domain.ProtoTransit.Entities.Messages.Data.Unsubscribe;

public sealed partial class Unsubscribe : ProtoTransit
{
    private Unsubscribe() : base(new ProtoHeader(MessageTypesEnum.Unsubscribe))
    {

    }
}