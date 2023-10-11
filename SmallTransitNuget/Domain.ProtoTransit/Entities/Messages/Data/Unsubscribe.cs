namespace Domain.ProtoTransit.Entities.Messages.Data;

internal sealed class Unsubscribe : Protocol
{
    public Unsubscribe() : base(MessageTypesEnum.Unsubscribe)
    {

    }
}