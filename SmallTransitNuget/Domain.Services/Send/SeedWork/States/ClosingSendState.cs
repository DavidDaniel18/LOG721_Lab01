using Domain.ProtoTransit;
using Domain.Services.Send.SeedWork.StateHolder;

namespace Domain.Services.Send.SeedWork.States;

internal sealed class ClosingSendState<TContext, TPayload> : SendState<TContext, TPayload> where TContext : SendStateHolder<TContext, TPayload>
{
    private protected override Func<SendState<TContext, TPayload>> OnAck { get; }
    private protected override Func<SendState<TContext, TPayload>> OnNack { get; }
    private protected override Func<SendState<TContext, TPayload>> OnInternalError { get; }
    private protected override Func<SendState<TContext, TPayload>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.Close;


    public ClosingSendState(TContext pushContext) : base(pushContext)
    {
        var closedState = new ClosedSendState<TContext, TPayload>(Context);

        OnAck = () => closedState;
        OnNack = () => closedState;
        OnInternalError = () => closedState;
        OnConnectionClosed = () => closedState;
    }
}