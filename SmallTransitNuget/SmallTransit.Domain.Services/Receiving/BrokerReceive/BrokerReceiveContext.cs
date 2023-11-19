using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Receiving.BrokerReceive.States;
using SmallTransit.Domain.Services.Receiving.States;

namespace SmallTransit.Domain.Services.Receiving.BrokerReceive;

internal sealed class BrokerReceiveContext : ReceiveStateHolder<BrokerReceiveContext, Protocol, BrokerReceiveResult, ReceivePublishByteWrapper>
{
    private protected override ReceiveState<BrokerReceiveContext, Protocol, BrokerReceiveResult, ReceivePublishByteWrapper> State { get; set; }

    public BrokerReceiveContext() { State = new FreeBrokerState(this); }

    internal override bool GetConnectionReady() => State is BrokerSubscribedState;
}