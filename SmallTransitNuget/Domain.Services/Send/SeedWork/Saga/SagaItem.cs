using Domain.ProtoTransit;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.SeedWork.Saga;

internal sealed record SagaItem<TContext, TPayload>(
    Protocol Message,
    Func<SendState<TContext, TPayload>> OnAck,
    Func<SendState<TContext, TPayload>> OnNack,
    Func<SendState<TContext, TPayload>> OnInternalError,
    Func<SendState<TContext, TPayload>> OnConnectionClosed);