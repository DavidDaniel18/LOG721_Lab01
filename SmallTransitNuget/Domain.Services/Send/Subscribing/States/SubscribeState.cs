using Domain.Services.Send.SeedWork.States;
using Domain.Services.Send.Subscribing.Dto;

namespace Domain.Services.Send.Subscribing.States;

internal abstract class SubscribeState : State<SubscribeContext, SubscriptionDto>
{
    protected SubscribeState(SubscribeContext context) : base(context)
    {
    }
}