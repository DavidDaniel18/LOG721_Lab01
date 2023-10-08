using Domain.Common;
using Domain.ProtoTransit;
using Domain.Services.Receive.States;

namespace Domain.Services.Receive.BrokerReceive.States;

internal sealed class BrokerSubscribedState : BrokerReceiveState
{
    public BrokerSubscribedState(BrokerReceiveContext context) : base(context)
    {
    }

    internal override Result<BrokerReceiveResult> Handle(Protocol payload)
    {
        return Result.Failure<BrokerReceiveResult>("Context should have been changed to push");
    }
}