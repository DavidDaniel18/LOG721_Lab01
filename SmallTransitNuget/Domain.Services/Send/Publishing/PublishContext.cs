using Domain.Services.Send.Publishing.States;
using Domain.Services.Send.SeedWork.StateHolder;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Publishing;

internal sealed class PublishContext : SendStateHolder<PublishContext, SerializedPublishMessage>
{
    private protected override State<PublishContext, SerializedPublishMessage> State { get; set; }

    public PublishContext() { State = new CreatedPublishingState(this); }

    internal override bool GetConnectionReady()
    {
        return State is OpenedPublishingState;
    }
}