namespace Domain.ProtoTransit.Entities.Messages.Data;

public sealed partial class Subscribe : ProtoTransit
{
    private Subscribe() : base(MessageTypesEnum.Subscribe) { }
}