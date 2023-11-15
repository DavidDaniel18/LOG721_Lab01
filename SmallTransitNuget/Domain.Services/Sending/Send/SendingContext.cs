using Domain.Services.Sending.SeedWork.StateHolder;
using Domain.Services.Sending.SeedWork.States;
using Domain.Services.Sending.Send.States;

namespace Domain.Services.Sending.Send;

internal sealed class SendingContext : SendingStateHolder<SendingContext, SerializedSendMessage>
{
    private protected override SendState<SendingContext, SerializedSendMessage> SendState { get; set; }

    public SendingContext() { SendState = new CreatedSendState(this); }

    internal override bool GetConnectionReady()
    {
        return SendState is OpenedSendState;
    }
}