using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Publishing.States;

internal abstract class PublishingState : State<PublishContext, SerializedPublishMessage>
{
    protected PublishingState(PublishContext pushContext) : base(pushContext) { }
}