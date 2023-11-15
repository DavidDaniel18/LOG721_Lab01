using Domain.ProtoTransit;
using Domain.Services.Receiving.States;

namespace Domain.Services.Receiving.SubscriberReceive.States;

internal abstract class SubscriberReceiveState : ReceiveState<SubscriberReceiveContext, Protocol, SubscriberReceiveResult, byte[]>
{
    protected SubscriberReceiveState(SubscriberReceiveContext context) : base(context)
    {
    }
}