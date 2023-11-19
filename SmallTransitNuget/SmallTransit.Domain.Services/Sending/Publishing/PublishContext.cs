using SmallTransit.Domain.Services.Sending.Publishing.States;
using SmallTransit.Domain.Services.Sending.SeedWork.StateHolder;
using SmallTransit.Domain.Services.Sending.SeedWork.States;

namespace SmallTransit.Domain.Services.Sending.Publishing;

internal sealed class PublishContext : SendingStateHolder<PublishContext, SerializedPublishMessage>
{
    private protected override SendState<PublishContext, SerializedPublishMessage> SendState { get; set; }

    public PublishContext() { SendState = new CreatedPublishingSendState(this); }

    internal override bool GetConnectionReady()
    {
        return SendState is OpenedPublishingSendState;
    }
}