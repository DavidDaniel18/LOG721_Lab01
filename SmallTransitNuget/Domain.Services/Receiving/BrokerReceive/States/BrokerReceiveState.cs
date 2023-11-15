using Domain.ProtoTransit;
using Domain.Services.Receiving.States;

namespace Domain.Services.Receiving.BrokerReceive.States;

internal abstract class BrokerReceiveState : ReceiveState<BrokerReceiveContext, Protocol, BrokerReceiveResult, ReceivePublishByteWrapper>
{
    protected BrokerReceiveState(BrokerReceiveContext context) : base(context) { }
}