﻿using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;
using SmallTransit.Domain.Services.Sending.SeedWork.Saga;
using SmallTransit.Domain.Services.Sending.SeedWork.States;

namespace SmallTransit.Domain.Services.Sending.Send.States;

internal sealed class OpenedSendState : SendState
{
    private protected override Func<SendState<SendingContext, SerializedSendMessage>> OnPayloadResponse { get; }
    private protected override Func<SendState<SendingContext, SerializedSendMessage>> OnAck { get; }
    private protected override Func<SendState<SendingContext, SerializedSendMessage>> OnNack { get; }
    private protected override Func<SendState<SendingContext, SerializedSendMessage>> OnInternalError { get; }
    private protected override Func<SendState<SendingContext, SerializedSendMessage>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.Send;

    public OpenedSendState(SendingContext sendingContext) : base(sendingContext)
    {
        var closingState = new ClosingSendState<SendingContext, SerializedSendMessage>(Context);

        OnPayloadResponse = () => this;
        OnAck = () => closingState;
        OnNack = () => closingState;
        OnInternalError = () => closingState;
        OnConnectionClosed = () => new ClosedSendState<SendingContext, SerializedSendMessage>(Context);
    }

    internal override Result<SagaItem<SendingContext, SerializedSendMessage>> BuildMessageSaga(SerializedSendMessage sendMessage)
    {
        var protoMessage = MessageFactory.Create(MessageTypesEnum.Send);

        return Result.From(
                protoMessage.TrySetProperty<SenderId>(sendMessage.SenderId),
                protoMessage.TrySetProperty<PayloadType>(sendMessage.SerializedPayloadType),
                protoMessage.TrySetProperty<Payload>(sendMessage.SerializedPayload))
            .Bind(() => GetSagaItem(protoMessage));
    }
}