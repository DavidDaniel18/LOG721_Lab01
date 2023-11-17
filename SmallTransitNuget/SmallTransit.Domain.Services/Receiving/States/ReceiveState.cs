using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.Services.Common;

namespace SmallTransit.Domain.Services.Receiving.States;

internal abstract class ReceiveState<TContext, TPayload, TStateResult, TResult> : Context
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