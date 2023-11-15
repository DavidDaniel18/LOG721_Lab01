using Domain.Services.Sending.SeedWork.States;

namespace Domain.Services.Sending.Push.States;

internal abstract class PushSendState : SendState<PushContext, byte[]>
{
    protected PushSendState(PushContext pushContext) : base(pushContext) { }
}