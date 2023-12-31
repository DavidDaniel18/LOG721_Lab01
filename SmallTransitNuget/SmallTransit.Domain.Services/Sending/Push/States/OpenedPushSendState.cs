﻿using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;
using SmallTransit.Domain.Services.Sending.SeedWork.Saga;
using SmallTransit.Domain.Services.Sending.SeedWork.States;

namespace SmallTransit.Domain.Services.Sending.Push.States;

internal sealed class OpenedPushSendState : PushSendState
{
    private protected override Func<SendState<PushContext, byte[]>> OnAck { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnPayloadResponse { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnNack { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnInternalError { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.Push;

    public OpenedPushSendState(PushContext pushContext) : base(pushContext)
    {
        var closingSendState = new ClosingSendState<PushContext, byte[]>(Context);

        OnPayloadResponse = () => closingSendState;
        OnAck = () => this;
        OnNack = () => this;
        OnInternalError = () => closingSendState;
        OnConnectionClosed = () => new ClosedSendState<PushContext, byte[]>(Context);
    }

    internal override Result<SagaItem<PushContext, byte[]>> BuildMessageSaga(byte[] payload)
    {
        var protoMessage = MessageFactory.Create(ResponseMessageType);

        return protoMessage.TrySetProperty<Payload>(payload).Bind(() => GetSagaItem(protoMessage));
    }
}