namespace SmallTransit.Domain.ProtoTransit.Entities.Messages.Monitoring;

internal sealed class Heartbeat : Protocol
{
    public Heartbeat() : base(MessageTypesEnum.Heartbeat) { }
}