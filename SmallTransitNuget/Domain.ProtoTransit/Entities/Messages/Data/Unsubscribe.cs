using Domain.ProtoTransit.Entities.Header;

namespace Domain.ProtoTransit.Entities.Messages.Data;

public sealed partial class Unsubscribe : ProtoTransit
{
    private Unsubscribe() : base(MessageTypesEnum.Unsubscribe)
    {

    }
}