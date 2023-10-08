﻿using Domain.Common;
using Domain.ProtoTransit;
using Domain.Services.Common;
using Domain.Services.Send.SeedWork.Saga;

namespace Domain.Services.Send.SeedWork.States;

internal abstract class State<TContext, TPayload>
{
    internal TContext Context { get; }
    internal ConnectionStateInfo? ConnectionStateInfo { get; set; }
    private protected abstract Func<State<TContext, TPayload>> OnAck { get; }
    private protected abstract Func<State<TContext, TPayload>> OnNack { get; }
    private protected abstract Func<State<TContext, TPayload>> OnInternalError { get; }
    private protected abstract Func<State<TContext, TPayload>> OnConnectionClosed { get; }
    private protected abstract MessageTypesEnum ResponseMessageType { get; }

    internal State(TContext context)
    {
        Context = context;
    }

    private Result<SagaItem<TContext, TPayload>> GetSagaItem() => Result.Success(new SagaItem<TContext, TPayload>(
        MessageFactory.Create(ResponseMessageType),
        OnAck,
        OnNack,
        OnInternalError,
        OnConnectionClosed));

    private protected Result<SagaItem<TContext, TPayload>> GetSagaItem(Protocol protoMessage) => Result.Success(new SagaItem<TContext, TPayload>(
        protoMessage,
        OnAck,
        OnNack,
        OnInternalError,
        OnConnectionClosed));

    internal virtual Result<SagaItem<TContext, TPayload>> BuildMessageSaga(TPayload payload) => GetSagaItem();
}