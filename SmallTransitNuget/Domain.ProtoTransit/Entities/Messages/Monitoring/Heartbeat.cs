namespace Domain.ProtoTransit.Entities.Messages.Monitoring;

internal sealed class Heartbeat : Protocol
{
    private Heartbeat() : base(MessageTypesEnum.Heartbeat) { }
}