using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Receiving.States;
using SmallTransit.Domain.Services.Receiving.SubscriberReceive.States;

namespace SmallTransit.Domain.Services.Receiving.SubscriberReceive;

internal sealed class SubscriberReceiveContext : ReceiveStateHolder<SubscriberReceiveContext, Protocol, SubscriberReceiveResult, byte[]>
{
    private protected override ReceiveState<SubscriberReceiveContext, Protocol, SubscriberReceiveResult, byte[]> State { get; set; }

    public SubscriberReceiveContext()
    {
        State = new SubscriberSubscribedState(this);
    }

    internal override bool GetConnectionReady()
    {
        throw new NotImplementedException();
    }
}