using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Sending.SeedWork.Saga;
using SmallTransit.Domain.Services.Sending.SeedWork.States;

namespace SmallTransit.Domain.Services.Sending.SeedWork.StateHolder;

internal abstract class SendingStateHolder<TContext, TPayload> : Context where TContext : SendingStateHolder<TContext, TPayload>
{
    private protected abstract SendState<TContext, TPayload> SendState { get; set; }

    internal void SetState(SendState<TContext, TPayload> sendState)
    {
        if (sendState != SendState)
            SendState = sendState;
    }

    internal Result<Saga<TContext, TPayload>> BuildSaga(TPayload payload)
    {
        return SendState.BuildMessageSaga(payload)
            .Bind(sagaItem =>
                Result.Success(new Saga<TContext, TPayload>((TContext)this, sagaItem, payload)));
    }

    internal abstract bool GetConnectionReady();
}