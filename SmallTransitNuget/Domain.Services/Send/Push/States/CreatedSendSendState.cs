using Domain.ProtoTransit;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Push.States;

internal sealed class CreatedSendState : PushState
{
    private protected override Func<State<PushContext, byte[]>> OnAck { get; }
    private protected override Func<State<PushContext, byte[]>> OnNack { get; }
    private protected override Func<State<PushContext, byte[]>> OnInternalError { get; }
    private protected override Func<State<PushContext, byte[]>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.HandShake;

    public CreatedSendState(PushContext pushContext) : base(pushContext)
    {
        OnAck = () => new OpenedSendState(pushContext);
        OnNack = () => this;
        OnInternalError = () => new ClosingSendState<PushContext, byte[]>(pushContext);
        OnConnectionClosed = () => new ClosedSendState<PushContext, byte[]>(Context);
    }
}