using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Receiving.States;

namespace SmallTransit.Domain.Services.Receiving.BrokerReceive.States;

internal abstract class BrokerReceiveState : ReceiveState<BrokerReceiveContext, Protocol, BrokerReceiveResult, ReceivePublishByteWrapper>
{
    protected BrokerReceiveState(BrokerReceiveContext context) : base(context) { }
}