using Domain.Common;
using Domain.Services.Common;
using Domain.Services.Send.SeedWork.Saga;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.SeedWork.StateHolder;

internal abstract class SendStateHolder<TContext, TPayload> : Context where TContext : SendStateHolder<TContext, TPayload>
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