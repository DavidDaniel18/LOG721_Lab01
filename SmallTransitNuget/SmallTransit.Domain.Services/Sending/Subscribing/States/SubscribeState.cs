using SmallTransit.Domain.Services.Sending.SeedWork.States;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;

namespace SmallTransit.Domain.Services.Sending.Subscribing.States;

internal abstract class SubscribeSendState : SendState<SubscribeContext, SubscriptionDto>
{
    protected SubscribeSendState(SubscribeContext context) : base(context)
    {
    }
}