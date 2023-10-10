using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Publishing.States;

internal abstract class PublishingSendState : SendState<PublishContext, SerializedPublishMessage>
{
    protected PublishingSendState(PublishContext pushContext) : base(pushContext) { }
}