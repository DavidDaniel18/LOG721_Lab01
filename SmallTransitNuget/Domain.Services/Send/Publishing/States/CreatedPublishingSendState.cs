using Domain.ProtoTransit;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Publishing.States;

internal sealed class CreatedPublishingState : PublishingState
{
    private protected override Func<State<PublishContext, SerializedPublishMessage>> OnAck { get; }
    private protected override Func<State<PublishContext, SerializedPublishMessage>> OnNack { get; }
    private protected override Func<State<PublishContext, SerializedPublishMessage>> OnInternalError { get; }
    private protected override Func<State<PublishContext, SerializedPublishMessage>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.HandShake;

    public CreatedPublishingState(PublishContext publishContext) : base(publishContext)
    {
        OnAck = () => new OpenedPublishingState(Context);
        OnNack = () => this;
        OnInternalError = () => new ClosingSendState<PublishContext, SerializedPublishMessage>(Context);
        OnConnectionClosed = () => new ClosedSendState<PublishContext, SerializedPublishMessage>(Context);
    }
}