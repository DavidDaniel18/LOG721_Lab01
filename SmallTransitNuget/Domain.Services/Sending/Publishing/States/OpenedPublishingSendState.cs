using Domain.Common.Monads;
using Domain.ProtoTransit;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Sending.SeedWork.Saga;
using Domain.Services.Sending.SeedWork.States;

namespace Domain.Services.Sending.Publishing.States;

internal sealed class OpenedPublishingSendState : PublishingSendState
{
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnAck { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnPayloadResponse { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnNack { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnInternalError { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.HandShake;

    public OpenedPublishingSendState(PublishContext publishContext) : base(publishContext)
    {
        var closingState = new ClosingSendState<PublishContext, SerializedPublishMessage>(Context);

        OnPayloadResponse = () => closingState;
        OnAck = () => this;
        OnNack = () => closingState;
        OnInternalError = () => closingState;
        OnConnectionClosed = () => new ClosedSendState<PublishContext, SerializedPublishMessage>(Context);
    }

    internal override Result<SagaItem<PublishContext, SerializedPublishMessage>> BuildMessageSaga(SerializedPublishMessage publishMessage)
    {
        var protoMessage = MessageFactory.Create(MessageTypesEnum.Publish);

        return Result.From(
            protoMessage.TrySetProperty<RoutingKey>(publishMessage.SerializedRoutingKey),
            protoMessage.TrySetProperty<PayloadType>(publishMessage.SerializedPayloadType),
            protoMessage.TrySetProperty<Payload>(publishMessage.SerializedPayload))
            .Bind(() => GetSagaItem(protoMessage));
    }
}