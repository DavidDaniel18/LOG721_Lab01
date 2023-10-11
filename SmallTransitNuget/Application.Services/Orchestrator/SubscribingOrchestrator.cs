using Domain.Common;
using Domain.Services.Common;
using Domain.Services.Receive.ClientReceive;
using Domain.Services.Send;
using Domain.Services.Send.Subscribing;
using Domain.Services.Send.Subscribing.Dto;
using MessagePack;

namespace Application.Services.Orchestrator;

internal sealed class SubscribingOrchestrator : SendOrchestrator<SubscribeContext, SubscriptionDto>
{
    private protected override SubscribeContext Context { get; }

    public SubscribingOrchestrator(SubscribeContext context, IComHandler comHandler) : base(comHandler)
    {
        Context = context;
    }

    internal async Task<Result<ContextAlterationRequests>> Execute(SubscriptionWrapper subscriptionWrapper) 
        => await Serialize(subscriptionWrapper).BindAsync(PrimeConnection).BindAsync(() => Result.Success(new ContextAlterationRequests(typeof(ClientReceiveContext))));

    private static Result<SubscriptionDto> Serialize(SubscriptionWrapper subscriptionWrapper)
    {
        try
        {
            var serializedRoutingKey = MessagePackSerializer.Serialize(subscriptionWrapper.RoutingKey);
            var serializedPayloadType = MessagePackSerializer.Serialize(subscriptionWrapper.PayloadType);
            var queueName = MessagePackSerializer.Serialize(subscriptionWrapper.QueueName);

            return Result.Success(new SubscriptionDto(serializedRoutingKey, serializedPayloadType, queueName));
        }
        catch (Exception e)
        {
            return Result.Failure<SubscriptionDto>(e);
        }
    }
}