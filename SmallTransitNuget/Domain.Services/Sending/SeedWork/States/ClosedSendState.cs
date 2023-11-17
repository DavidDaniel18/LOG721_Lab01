using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Common.Exceptions;
using SmallTransit.Domain.Services.Sending.SeedWork.Saga;
using SmallTransit.Domain.Services.Sending.SeedWork.StateHolder;

namespace SmallTransit.Domain.Services.Sending.SeedWork.States;

internal sealed class ClosedSendState<TContext, TPayload> : SendState<TContext, TPayload> where TContext : SendingStateHolder<TContext, TPayload>
{
    private protected override Func<SendState<TContext, TPayload>> OnAck => null!;
    private protected override Func<SendState<TContext, TPayload>> OnPayloadResponse { get; }
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