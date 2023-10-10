using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Send.SeedWork.Saga;
using Domain.Services.Send.SeedWork.States;

namespace Domain.Services.Send.Push.States;

internal sealed class OpenedSendState : PushSendState
{
    private protected override Func<SendState<PushContext, byte[]>> OnAck { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnNack { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnInternalError { get; }
    private protected override Func<SendState<PushContext, byte[]>> OnConnectionClosed { get; }
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