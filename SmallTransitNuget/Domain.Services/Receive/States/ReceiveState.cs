using Domain.Common;

namespace Domain.Services.Receive.States;

internal abstract class ReceiveState<TContext, TPayload, TStateResult, TResult> 
    where TContext : ReceiveStateHolder<TContext, TPayload, TStateResult, TResult>
    where TStateResult : StateResult<TResult>
{
    internal TContext Context { get; }

    internal ReceiveState(TContext context)
    {
        Context = context;
    }

    internal abstract Result<TStateResult> Handle(TPayload payload);

}