using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Push.States;

internal abstract class PushSendState : SendState<PushContext, byte[]>
{
    protected PushSendState(PushContext pushContext) : base(pushContext) { }
}