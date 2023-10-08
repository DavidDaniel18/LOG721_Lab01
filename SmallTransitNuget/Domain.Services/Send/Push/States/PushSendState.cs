using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Push.States;

internal abstract class PushState : State<PushContext, byte[]>
{
    protected PushState(PushContext pushContext) : base(pushContext) { }
}