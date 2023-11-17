using SmallTransit.Domain.Services.Sending.SeedWork.States;

namespace SmallTransit.Domain.Services.Sending.Push.States;

internal abstract class PushSendState : SendState<PushContext, byte[]>
{
    protected PushSendState(PushContext pushContext) : base(pushContext) { }
}