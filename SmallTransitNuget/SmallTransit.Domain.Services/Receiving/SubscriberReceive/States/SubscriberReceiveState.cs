using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Receiving.States;

namespace SmallTransit.Domain.Services.Receiving.SubscriberReceive.States;

internal abstract class SubscriberReceiveState : ReceiveState<SubscriberReceiveContext, Protocol, SubscriberReceiveResult, byte[]>
{
    protected SubscriberReceiveState(SubscriberReceiveContext context) : base(context)
    {
    }
}