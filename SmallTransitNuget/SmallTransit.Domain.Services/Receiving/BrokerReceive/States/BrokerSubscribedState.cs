using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Receiving.States;

namespace SmallTransit.Domain.Services.Receiving.BrokerReceive.States;

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