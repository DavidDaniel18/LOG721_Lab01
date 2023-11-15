using Domain.ProtoTransit;
using Domain.Services.Sending.SeedWork.States;

namespace Domain.Services.Sending.SeedWork.Saga;

internal sealed record SagaItem<TContext, TPayload>(
    Protocol Message,
    Func<SendState<TContext, TPayload>> OnPayloadResponse,
    Func<SendState<TContext, TPayload>> OnAck,
    Func<SendState<TContext, TPayload>> OnNack,
    Func<SendState<TContext, TPayload>> OnInternalError,
    Func<SendState<TContext, TPayload>> OnConnectionClosed);