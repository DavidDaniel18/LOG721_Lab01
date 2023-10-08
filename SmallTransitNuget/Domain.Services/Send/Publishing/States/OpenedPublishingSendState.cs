using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Send.SeedWork.Saga;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Publishing.States;

internal sealed class OpenedPublishingState : PublishingState
{
    private protected override Func<State<PublishContext, SerializedPublishMessage>> OnAck { get; }
    private protected override Func<State<PublishContext, SerializedPublishMessage>> OnNack { get; }
    private protected override Func<State<PublishContext, SerializedPublishMessage>> OnInternalError { get; }
    private protected override Func<State<PublishContext, SerializedPublishMessage>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.HandShake;

    public OpenedPublishingState(PublishContext publishContext) : base(publishContext)
    {
        var closingState = new ClosingSendState<PublishContext, SerializedPublishMessage>(Context);

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