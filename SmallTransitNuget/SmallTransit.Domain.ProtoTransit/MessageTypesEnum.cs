using SmallTransit.Domain.ProtoTransit.Entities.Messages.Core;
using SmallTransit.Domain.ProtoTransit.Entities.Messages.Data;
using SmallTransit.Domain.ProtoTransit.Entities.Messages.Monitoring;
using SmallTransit.Domain.ProtoTransit.Seedwork;

namespace SmallTransit.Domain.ProtoTransit;

public enum MessageTypesEnum
{
    [MessageType<Ack>]
    Ack,
    [MessageType<Nack>]
    Nack,
    [MessageType<HandShake>]
    HandShake,
    [MessageType<Publish>]
    Publish,
    [MessageType<Push>]
    Push,
    [MessageType<Subscribe>]
    Subscribe,
    [MessageType<Unsubscribe>]
    Unsubscribe,
    [MessageType<Heartbeat>]
    Heartbeat,
    [MessageType<Close>]
    Close,
    [MessageType<Send>]
    Send,
    [MessageType<PayloadResponse>]
    PayloadResponse
}