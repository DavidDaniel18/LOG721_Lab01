using Domain.Services.Send.Publishing.States;
using Domain.Services.Send.SeedWork.StateHolder;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Publishing;

internal sealed class PublishContext : SendStateHolder<PublishContext, SerializedPublishMessage>
{
    private protected override SendState<PublishContext, SerializedPublishMessage> SendState { get; set; }

    public PublishContext() { SendState = new CreatedPublishingSendState(this); }

    internal override bool GetConnectionReady()
    {
        return SendState is OpenedPublishingSendState;
    }
}