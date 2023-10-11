using Domain.Common;
using Domain.ProtoTransit;

namespace Domain.Services.Receive.States;

internal sealed class BrokerClosedState<TContext, TResult, TReceive> : ReceiveState<TContext, Protocol, TResult, TReceive> 
    where TContext : ReceiveStateHolder<TContext, Protocol, TResult, TReceive>
    where TResult : StateResult<TReceive>
{
    internal BrokerClosedState(TContext context) : base(context) { }

    internal override Result<TResult> Handle(Protocol payload)
    {
        return Result.Failure<TResult>("Connection is closed");
    }
}