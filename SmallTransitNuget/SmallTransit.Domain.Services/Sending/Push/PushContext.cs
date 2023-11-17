using SmallTransit.Domain.Services.Sending.Push.States;
using SmallTransit.Domain.Services.Sending.SeedWork.StateHolder;
using SmallTransit.Domain.Services.Sending.SeedWork.States;

namespace SmallTransit.Domain.Services.Sending.Push;

internal sealed class PushContext : SendingStateHolder<PushContext, byte[]>
{
    private protected override SendState<PushContext, byte[]> SendState { get; set; }

    public PushContext() { SendState = new OpenedPushSendState(this); }

    internal override bool GetConnectionReady() => SendState is OpenedPushSendState;
}