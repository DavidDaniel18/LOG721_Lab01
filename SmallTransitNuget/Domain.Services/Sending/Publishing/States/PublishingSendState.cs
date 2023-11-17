using SmallTransit.Domain.Services.Sending.SeedWork.States;

namespace SmallTransit.Domain.Services.Sending.Publishing.States;

internal abstract class PublishingSendState : SendState<PublishContext, SerializedPublishMessage>
{
    protected PublishingSendState(PublishContext pushContext) : base(pushContext) { }
}