using Domain.ProtoTransit.Entities.Messages.Core;
using Domain.ProtoTransit.Entities.Messages.Data;
using Domain.ProtoTransit.Entities.Messages.Monitoring;
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
    [MessageTypeAttribute<Push>]
    Push,
    [MessageTypeAttribute<Subscribe>]
    Subscribe,
    [MessageTypeAttribute<Unsubscribe>]
    Unsubscribe,
    [MessageTypeAttribute<Heartbeat>]
    Heartbeat,
    [MessageTypeAttribute<Close>]
    Close
}