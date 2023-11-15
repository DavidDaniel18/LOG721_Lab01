using Domain.Services.Sending.Push.States;
using Domain.Services.Sending.SeedWork.StateHolder;
using Domain.Services.Sending.SeedWork.States;

namespace Domain.Services.Sending.Push;

internal sealed class PushContext : SendingStateHolder<PushContext, byte[]>
{
    private protected override SendState<PushContext, byte[]> SendState { get; set; }

    public PushContext() { SendState = new OpenedPushSendState(this); }

    internal override bool GetConnectionReady() => SendState is OpenedPushSendState;
}