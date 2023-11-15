using Domain.Services.Sending.Publishing.States;
using Domain.Services.Sending.SeedWork.StateHolder;
using Domain.Services.Sending.SeedWork.States;

namespace Domain.Services.Sending.Publishing;

internal sealed class PublishContext : SendingStateHolder<PublishContext, SerializedPublishMessage>
{
    private protected override SendState<PublishContext, SerializedPublishMessage> SendState { get; set; }

    public PublishContext() { SendState = new CreatedPublishingSendState(this); }

    internal override bool GetConnectionReady()
    {
        return SendState is OpenedPublishingSendState;
    }
}