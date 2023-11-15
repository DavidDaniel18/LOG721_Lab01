using Domain.Services.Sending.SeedWork.States;

namespace Domain.Services.Sending.Send.States;

internal abstract class SendState : SendState<SendingContext, SerializedSendMessage>
{
    protected SendState(SendingContext sendingContext) : base(sendingContext) { }
}