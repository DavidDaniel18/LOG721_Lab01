using Domain.ProtoTransit;
using Domain.Services.Sending.SeedWork.States;

namespace Domain.Services.Sending.Publishing.States;

internal sealed class CreatedPublishingSendState : PublishingSendState
{
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnAck { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnPayloadResponse { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnNack { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnInternalError { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.HandShake;

    public CreatedPublishingSendState(PublishContext publishContext) : base(publishContext)
    {
        var closingSendState = new ClosingSendState<PublishContext, SerializedPublishMessage>(Context);

        OnPayloadResponse = () => closingSendState;
        OnAck = () => new OpenedPublishingSendState(Context);
        OnNack = () => this;
        OnInternalError = () => closingSendState;
        OnConnectionClosed = () => new ClosedSendState<PublishContext, SerializedPublishMessage>(Context);
    }
}