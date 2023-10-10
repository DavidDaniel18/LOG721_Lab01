using Domain.Services.Send.SeedWork.States;
using Domain.Services.Send.Subscribing.Dto;

namespace Domain.Services.Send.Subscribing.States;

internal abstract class SubscribeSendState : SendState<SubscribeContext, SubscriptionDto>
{
    protected SubscribeSendState(SubscribeContext context) : base(context)
    {
    }
}