using SmallTransit.Abstractions.Monads;

namespace SmallTransit.Domain.Services.Receiving.States;

internal abstract class ReceiveStateHolder<TContext, TPayload, TStateResult, TResult> 
    where TContext : ReceiveStateHolder<TContext, TPayload, TStateResult, TResult>
    where TStateResult : StateResult<TResult>
{
    private protected abstract ReceiveState<TContext, TPayload, TStateResult, TResult> State { get; set; }

    internal void SetState(ReceiveState<TContext, TPayload, TStateResult, TResult> state)
    {
        if (state != State)
            State = state;
    }

    internal Result<TStateResult> Handle(TPayload payload)
    {
        return State.Handle(payload);
    }

    internal abstract bool GetConnectionReady();
}