using Domain.ProtoTransit.Seedwork;

namespace Domain.ProtoTransit;

public enum MessageTypesEnum
{
    [MessageTypeAttribute<Ack>]
    Ack,
    [MessageTypeAttribute<Nack>]
    Nack,
    [MessageTypeAttribute<HandShake>]
    HandShake,
    [MessageTypeAttribute<Publish>]
    Publish,
    [MessageTypeAttribute<Subscribe>]
    Subscribe,
    [MessageTypeAttribute<Unsubscribe>]
    Unsubscribe,
    [MessageTypeAttribute<Heartbeat>]
    Heartbeat
}