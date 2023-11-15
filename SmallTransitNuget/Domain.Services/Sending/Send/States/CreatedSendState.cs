using Domain.ProtoTransit;
using Domain.Services.Sending.SeedWork.States;

namespace Domain.Services.Sending.Send.States;

internal sealed class CreatedSendState : SendState
{
    private protected override Func<SendState<SendingContext, SerializedSendMessage>> OnAck { get; }
    private protected override Func<SendState<SendingContext, SerializedSendMessage>> OnPayloadResponse { get; }
    private protected override Func<SendState<SendingContext, SerializedSendMessage>> OnNack { get; }
    private protected override Func<SendState<SendingContext, SerializedSendMessage>> OnInternalError { get; }
    private protected override Func<SendState<SendingContext, SerializedSendMessage>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.HandShake;

    public CreatedSendState(SendingContext sendingContext) : base(sendingContext)
    {
        var closingSendState = new ClosingSendState<SendingContext, SerializedSendMessage>(Context);

        OnPayloadResponse = () => closingSendState;
        OnAck = () => new OpenedSendState(Context);
        OnNack = () => this;
        OnInternalError = () => closingSendState;
        OnConnectionClosed = () => new ClosedSendState<SendingContext, SerializedSendMessage>(Context);
    }
}