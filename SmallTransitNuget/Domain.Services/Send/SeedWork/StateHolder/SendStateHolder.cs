using Domain.Common;
using Domain.Services.Send.SeedWork.Saga;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.SeedWork.StateHolder;

internal abstract class SendStateHolder<TContext, TPayload> where TContext : SendStateHolder<TContext, TPayload>
{
    private protected abstract State<TContext, TPayload> State { get; set; }

    internal void SetState(State<TContext, TPayload> state)
    {
        if (state != State)
            State = state;
    }

    internal Result<Saga<TContext, TPayload>> BuildSaga(TPayload payload)
    {
        return State.BuildMessageSaga(payload)
            .Bind(sagaItem =>
                Result.Success(new Saga<TContext, TPayload>((TContext)this, sagaItem, payload)));
    }

    internal abstract bool GetConnectionReady();
}