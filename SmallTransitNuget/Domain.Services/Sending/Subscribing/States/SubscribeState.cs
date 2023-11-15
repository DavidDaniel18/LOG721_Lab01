using Domain.Services.Sending.SeedWork.States;
using Domain.Services.Sending.Subscribing.Dto;

namespace Domain.Services.Sending.Subscribing.States;

internal abstract class SubscribeSendState : SendState<SubscribeContext, SubscriptionDto>
{
    protected SubscribeSendState(SubscribeContext context) : base(context)
    {
    }
}