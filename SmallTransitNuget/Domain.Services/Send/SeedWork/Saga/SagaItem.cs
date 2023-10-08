using Domain.ProtoTransit;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.SeedWork.Saga;

internal sealed record SagaItem<TContext, TPayload>(
    Protocol Message,
    Func<State<TContext, TPayload>> OnAck,
    Func<State<TContext, TPayload>> OnNack,
    Func<State<TContext, TPayload>> OnInternalError,
    Func<State<TContext, TPayload>> OnConnectionClosed);