using Domain.Services.Sending.SeedWork.StateHolder;
using Domain.Services.Sending.SeedWork.States;
using Domain.Services.Sending.Subscribing.Dto;
using Domain.Services.Sending.Subscribing.States;

namespace Domain.Services.Sending.Subscribing;

internal sealed class SubscribeContext : SendingStateHolder<SubscribeContext, SubscriptionDto>
{
    private protected override SendState<SubscribeContext, SubscriptionDto> SendState { get; set; }

    internal SubscribeContext()
    {
        SendState = new CreatedSubscribingSendState(this);
    }

    internal override bool GetConnectionReady()
    {
        return SendState is SubscribedSendState;
    }
}