using Domain.ProtoTransit.Entities.Header;

namespace Domain.ProtoTransit.Entities.Messages.Monitoring.Heartbeat;

public sealed class Heartbeat : ProtoTransit
{
    private Heartbeat() : base(new ProtoHeader(MessageTypesEnum.Heartbeat))
    {

    }
}