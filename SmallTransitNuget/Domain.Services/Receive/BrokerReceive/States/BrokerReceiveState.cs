using Domain.ProtoTransit;
using Domain.Services.Receive.States;

namespace Domain.Services.Receive.BrokerReceive.States;

internal abstract class BrokerReceiveState : ReceiveState<BrokerReceiveContext, Protocol, BrokerReceiveResult, PublishByteWrapper>
{
    protected BrokerReceiveState(BrokerReceiveContext context) : base(context) { }
}