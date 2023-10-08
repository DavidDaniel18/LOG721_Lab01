using Domain.Services.Send.Push.States;
using Domain.Services.Send.SeedWork.StateHolder;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Push;

internal sealed class PushContext : SendStateHolder<PushContext, byte[]>
{
    private protected override State<PushContext, byte[]> State { get; set; }

    public PushContext() { State = new CreatedSendState(this); }

    internal override bool GetConnectionReady() => State is OpenedSendState;
}