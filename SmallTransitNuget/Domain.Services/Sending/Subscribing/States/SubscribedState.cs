using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Sending.SeedWork.Saga;
using SmallTransit.Domain.Services.Sending.SeedWork.States;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;

namespace SmallTransit.Domain.Services.Sending.Subscribing.States;

internal sealed class SubscribedSendState : SubscribeSendState
{
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnAck { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnPayloadResponse { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnNack { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnInternalError { get; }
    private protected override Func<SendState<SubscribeContext, SubscriptionDto>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType { get; }

    public SubscribedSendState(SubscribeContext context) : base(context) { }

    internal override Result<SagaItem<SubscribeContext, SubscriptionDto>> BuildMessageSaga(SubscriptionDto payload)
    {
        return Result.Failure<SagaItem<SubscribeContext, SubscriptionDto>>("SubscribedState cannot build message saga");
    }
}