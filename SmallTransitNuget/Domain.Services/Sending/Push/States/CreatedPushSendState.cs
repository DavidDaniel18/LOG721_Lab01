using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Sending.SeedWork.States;

namespace SmallTransit.Domain.Services.Sending.Push.States;

internal sealed class CreatedPushSendState : PushSendState
{
    private protected override Func<SendState<PushContext, byte[]>> OnAck { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnPayloadResponse { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnNack { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnInternalError { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.HandShake;

    public CreatedPushSendState(PushContext pushContext) : base(pushContext)
    {
        var closingSendState = new ClosingSendState<PushContext, byte[]>(Context);

        OnPayloadResponse = () => closingSendState;
        OnAck = () => new OpenedPushSendState(pushContext);
        OnNack = () => this;
        OnInternalError = () => closingSendState;
        OnConnectionClosed = () => new ClosedSendState<PushContext, byte[]>(Context);
    }
}