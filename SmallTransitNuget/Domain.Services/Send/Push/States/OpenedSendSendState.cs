using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Common;
using Domain.Services.Send.Push.Exceptions;
using Domain.Services.Send.SeedWork.Saga;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Push.States;

internal sealed class OpenedSendState : PushState
{
    private protected override Func<State<PushContext, byte[]>> OnAck { get; }
    private protected override Func<State<PushContext, byte[]>> OnNack { get; }
    private protected override Func<State<PushContext, byte[]>> OnInternalError { get; }
    private protected override Func<State<PushContext, byte[]>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.Push;

    public OpenedSendState(PushContext pushContext) : base(pushContext)
    {
        OnAck = () => this;
        OnNack = () => this;
        OnInternalError = () => new ClosingSendState<PushContext, byte[]>(Context);
        OnConnectionClosed = () => new ClosedSendState<PushContext, byte[]>(Context);
    }

    internal override Result<SagaItem<PushContext, byte[]>> BuildMessageSaga(byte[] payload)
    {
        var protoMessage = MessageFactory.Create(ResponseMessageType);

        return protoMessage.TrySetProperty<Payload>(payload).Bind(() => GetSagaItem(protoMessage));
    }
}