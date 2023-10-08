using Domain.Services.Send.SeedWork.StateHolder;
using Domain.Services.Send.SeedWork.States;
using Domain.Services.Send.Subscribing.Dto;
using Domain.Services.Send.Subscribing.States;

namespace Domain.Services.Send.Subscribing;

internal sealed class SubscribeContext : SendStateHolder<SubscribeContext, SubscriptionDto>
{
    private protected override State<SubscribeContext, SubscriptionDto> State { get; set; }

    internal override bool GetConnectionReady()
    {
        return State is SubscribedState;
    }
}