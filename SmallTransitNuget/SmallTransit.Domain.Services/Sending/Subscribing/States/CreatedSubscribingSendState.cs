using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;
using SmallTransit.Domain.Services.Sending.SeedWork.Saga;
using SmallTransit.Domain.Services.Sending.SeedWork.States;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;

namespace SmallTransit.Domain.Services.Sending.Subscribing.States;

internal sealed class CreatedSubscribingSendState : SubscribeSendState
{
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnAck { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnPayloadResponse { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnNack { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnInternalError { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType => MessageTypesEnum.Subscribe;

    public CreatedSubscribingSendState(SubscribeContext publishContext) : base(publishContext)
    {
        var closingState = new ClosingSendState<SubscribeContext, SubscriptionDto>(Context);

        OnPayloadResponse = () => closingState;
        OnAck = () => new SubscribedSendState(Context);
        OnNack = () => this;
        OnInternalError = () => closingState;
        OnConnectionClosed = () => closingState;
    }

    internal override Result<SagaItem<SubscribeContext, SubscriptionDto>> BuildMessageSaga(SubscriptionDto payload)
    {
        var protoMessage = MessageFactory.Create(ResponseMessageType);

        return Result.From(
            protoMessage.TrySetProperty<RoutingKey>(payload.RoutingKey),
            protoMessage.TrySetProperty<PayloadType>(payload.PayloadType),
            protoMessage.TrySetProperty<QueueName>(payload.QueueName))
            .Bind<SagaItem<SubscribeContext, SubscriptionDto>>(() => Result.Success(new SagaItem<SubscribeContext, SubscriptionDto>(protoMessage,
                OnPayloadResponse,
                OnAck,
                OnNack,
                OnInternalError,
                OnConnectionClosed)));
    }
}