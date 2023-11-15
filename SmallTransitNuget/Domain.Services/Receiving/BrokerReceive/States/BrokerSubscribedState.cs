using Domain.Common.Monads;
using Domain.ProtoTransit;
using Domain.Services.Receiving.States;

namespace Domain.Services.Receiving.BrokerReceive.States;

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