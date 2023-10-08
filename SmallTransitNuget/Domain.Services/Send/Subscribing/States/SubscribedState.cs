using Domain.Common;
using Domain.ProtoTransit;
using Domain.Services.Send.SeedWork.Saga;
using Domain.Services.Send.SeedWork.States;
using Domain.Services.Send.Subscribing.Dto;

namespace Domain.Services.Send.Subscribing.States;

internal sealed class SubscribedState : SubscribeState
{
    private protected override Func<State<SubscribeContext, SubscriptionDto>> OnAck { get; }
    private protected override Func<State<SubscribeContext, SubscriptionDto>> OnNack { get; }
    private protected override Func<State<SubscribeContext, SubscriptionDto>> OnInternalError { get; }
    private protected override Func<State<SubscribeContext, SubscriptionDto>> OnConnectionClosed { get; }
    private protected override MessageTypesEnum ResponseMessageType { get; }

    public SubscribedState(SubscribeContext context) : base(context) { }

    internal override Result<SagaItem<SubscribeContext, SubscriptionDto>> BuildMessageSaga(SubscriptionDto payload)
    {
        return Result.Failure<SagaItem<SubscribeContext, SubscriptionDto>>("SubscribedState cannot build message saga");
    }
}