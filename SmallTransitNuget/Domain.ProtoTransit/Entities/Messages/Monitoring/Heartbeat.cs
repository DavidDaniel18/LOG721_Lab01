namespace Domain.ProtoTransit.Entities.Messages.Monitoring;

public sealed class Heartbeat : ProtoTransit
{
    private Heartbeat() : base(MessageTypesEnum.Heartbeat) { }
}