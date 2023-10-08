namespace Domain.ProtoTransit.Entities.Messages.Data;

internal sealed class Unsubscribe : Protocol
{
    private Unsubscribe() : base(MessageTypesEnum.Unsubscribe)
    {

    }
}