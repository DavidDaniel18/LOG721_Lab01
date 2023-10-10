using Domain.Services.Send.SeedWork.StateHolder;
using Domain.Services.Send.SeedWork.States;
using Domain.Services.Send.Subscribing.Dto;
using Domain.Services.Send.Subscribing.States;

namespace Domain.Services.Send.Subscribing;

internal sealed class SubscribeContext : SendStateHolder<SubscribeContext, SubscriptionDto>
{
    private protected override SendState<SubscribeContext, SubscriptionDto> SendState { get; set; }

    internal override bool GetConnectionReady()
    {
        return SendState is SubscribedSendState;
    }
}