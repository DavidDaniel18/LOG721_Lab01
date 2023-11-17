using SmallTransit.Domain.Services.Sending.SeedWork.StateHolder;
using SmallTransit.Domain.Services.Sending.SeedWork.States;
using SmallTransit.Domain.Services.Sending.Send.States;

namespace SmallTransit.Domain.Services.Sending.Send;

internal sealed class SendingContext : SendingStateHolder<SendingContext, SerializedSendMessage>
{
    private protected override SendState<SendingContext, SerializedSendMessage> SendState { get; set; }

    public SendingContext() { SendState = new OpenedSendState(this); }

    internal override bool GetConnectionReady()
    {
        return SendState is OpenedSendState;
    }
}