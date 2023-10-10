using Domain.Services.Send.Push.States;
using Domain.Services.Send.SeedWork.StateHolder;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Push;

internal sealed class PushContext : SendStateHolder<PushContext, byte[]>
{
    private protected override SendState<PushContext, byte[]> SendState { get; set; }

    public PushContext() { SendState = new CreatedSendState(this); }

    internal override bool GetConnectionReady() => SendState is OpenedSendState;
}