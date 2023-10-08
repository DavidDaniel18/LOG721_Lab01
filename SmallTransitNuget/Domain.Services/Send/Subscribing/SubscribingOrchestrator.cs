using Domain.Common;
using Domain.Services.Common;
using Domain.Services.Receive.ClientReceive;
using Domain.Services.Send.Publishing;
using Domain.Services.Send.Subscribing.Dto;
using MessagePack;

namespace Domain.Services.Send.Subscribing;

internal sealed class SubscribingOrchestrator : SendOrchestrator<SubscribeContext, SubscriptionDto>
{
    private protected override SubscribeContext Context { get; }

    public SubscribingOrchestrator(IComHandler comHandler, SubscribeContext context) : base(comHandler)
    {
        Context = context;
    }

    internal async Task<Result<ContextAlterationRequests>> Execute(SubscriptionWrapper subscriptionWrapper) 
        => await Serialize(subscriptionWrapper).BindAsync(PrimeConnection).BindAsync(() => Result.Success(new ContextAlterationRequests(typeof(ClientReceiveContext<>))));

    private static Result<SubscriptionDto> Serialize(SubscriptionWrapper subscriptionWrapper)
    {
        try
        {
            var serializedRoutingKey = MessagePackSerializer.Serialize(subscriptionWrapper.RoutingKey);
            var serializedPayloadType = MessagePackSerializer.Serialize(subscriptionWrapper.PayloadType.GetType());
            var queueName = MessagePackSerializer.Serialize(subscriptionWrapper.QueueName);

            return Result.Success(new SubscriptionDto(serializedRoutingKey, serializedPayloadType, queueName));
        }
        catch (Exception e)
        {
            return Result.Failure<SubscriptionDto>(e);
        }
    }
}