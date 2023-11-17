using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Sending.SeedWork.Saga;

namespace SmallTransit.Domain.Services.Sending.SeedWork.States;

internal abstract class SendState<TContext, TPayload>
{
    internal TContext Context { get; }
    internal ConnectionStateInfo? ConnectionStateInfo { get; set; }
    private protected abstract Func<SendState<TContext, TPayload>> OnAck { get; }
    private protected abstract Func<SendState<TContext, TPayload>> OnPayloadResponse { get; }
    private protected abstract Func<SendState<TContext, TPayload>> OnNack { get; }
    private protected abstract Func<SendState<TContext, TPayload>> OnInternalError { get; }
    private protected abstract Func<SendState<TContext, TPayload>> OnConnectionClosed { get; }
    private protected abstract MessageTypesEnum ResponseMessageType { get; }

    internal SendState(TContext context)
    {
        Context = context;
    }

    private Result<SagaItem<TContext, TPayload>> GetSagaItem() => Result.Success(new SagaItem<TContext, TPayload>(
        MessageFactory.Create(ResponseMessageType),
        OnPayloadResponse,
        OnAck,
        OnNack,
        OnInternalError,
        OnConnectionClosed));

    private protected Result<SagaItem<TContext, TPayload>> GetSagaItem(Protocol protoMessage) => Result.Success(new SagaItem<TContext, TPayload>(
        protoMessage,
        OnPayloadResponse,
        OnAck,
        OnNack,
        OnInternalError,
        OnConnectionClosed));

    internal virtual Result<SagaItem<TContext, TPayload>> BuildMessageSaga(TPayload payload) => GetSagaItem();
}