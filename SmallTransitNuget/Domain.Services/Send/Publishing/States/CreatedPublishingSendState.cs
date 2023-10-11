using Domain.ProtoTransit;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Publishing.States;

internal sealed class CreatedPublishingSendState : PublishingSendState
{
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnAck { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnNack { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnInternalError { get; }
    private protected override Func<SendState<PublishContext, SerializedPublishMessage>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.HandShake;

    public CreatedPublishingSendState(PublishContext publishContext) : base(publishContext)
    {
        OnAck = () => new OpenedPublishingSendState(Context);
        OnNack = () => this;
        OnInternalError = () => new ClosingSendState<PublishContext, SerializedPublishMessage>(Context);
        OnConnectionClosed = () => new ClosedSendState<PublishContext, SerializedPublishMessage>(Context);
    }
}