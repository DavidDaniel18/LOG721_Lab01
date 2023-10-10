using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Send.SeedWork.Saga;
using Domain.Services.Send.SeedWork.States;
using Domain.Services.Send.Subscribing.Dto;

namespace Domain.Services.Send.Subscribing.States;

internal sealed class CreatedSubscribingSendState : SubscribeSendState
{
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnAck { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnNack { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnInternalError { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.Subscribe;

    public CreatedSubscribingSendState(SubscribeContext publishContext) : base(publishContext)
    {
        OnAck = () => new SubscribedSendState(Context);
        OnNack = () => this;
        OnInternalError = () => new ClosingSendState<SubscribeContext, SubscriptionDto>(Context);
        OnConnectionClosed = () => new ClosedSendState<SubscribeContext, SubscriptionDto>(Context);
    }

    internal override Result<SagaItem<SubscribeContext, SubscriptionDto>> BuildMessageSaga(SubscriptionDto payload)
    {
        var protoMessage = MessageFactory.Create(ResponseMessageType);

        return Result.From(
            protoMessage.TrySetProperty<RoutingKey>(payload.RoutingKey),
            protoMessage.TrySetProperty<PayloadType>(payload.PayloadType),
            protoMessage.TrySetProperty<QueueName>(payload.QueueName))
            .Bind(() => Result.Success(new SagaItem<SubscribeContext, SubscriptionDto>(protoMessage,
                OnAck,
                OnNack,
                OnInternalError,
                OnConnectionClosed)));
    }
}