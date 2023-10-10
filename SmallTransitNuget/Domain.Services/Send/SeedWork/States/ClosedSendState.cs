using Domain.Common;
using Domain.ProtoTransit;
using Domain.Services.Common.Exceptions;
using Domain.Services.Send.SeedWork.Saga;
using Domain.Services.Send.SeedWork.StateHolder;

namespace Domain.Services.Send.SeedWork.States;

internal sealed class ClosedSendState<TContext, TPayload> : SendState<TContext, TPayload> where TContext : SendStateHolder<TContext, TPayload>
{
    private protected override Func<SendState<TContext, TPayload>> OnAck => null!;
    private protected override Func<SendState<TContext, TPayload>> OnNack => null!;
    private protected override Func<SendState<TContext, TPayload>> OnInternalError => null!;
    private protected override Func<SendState<TContext, TPayload>> OnConnectionClosed => null!;
    private protected override MessageTypesEnum ResponseMessageType => throw new NotImplementedException();

    public ClosedSendState(TContext pushContext) : base(pushContext) { }

    internal override Result<SagaItem<TContext, TPayload>> BuildMessageSaga(TPayload payload)
    {
        return Result.Failure<SagaItem<TContext, TPayload>>(new ConnectionClosedException());
    }
}