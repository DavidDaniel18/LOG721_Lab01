using SmallTransit.Domain.Services.Sending.SeedWork.StateHolder;
using SmallTransit.Domain.Services.Sending.SeedWork.States;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;
using SmallTransit.Domain.Services.Sending.Subscribing.States;

namespace SmallTransit.Domain.Services.Sending.Subscribing;

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