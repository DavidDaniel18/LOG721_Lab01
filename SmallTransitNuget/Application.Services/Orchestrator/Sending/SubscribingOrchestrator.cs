using Domain.Common.Monads;
using Domain.Services.Common;
using Domain.Services.Receiving.SubscriberReceive;
using Domain.Services.Sending;
using Domain.Services.Sending.Subscribing;
using Domain.Services.Sending.Subscribing.Dto;
using MessagePack;

namespace Application.Services.Orchestrator.Sending;

internal sealed class SubscribingOrchestrator : SendingOrchestrator<SubscribeContext, SubscriptionDto>
{
    private protected override SubscribeContext Context { get; }

    public SubscribingOrchestrator(SubscribeContext context, IComHandler comHandler) : base(comHandler)
    {
        Context = context;
    }

    internal async Task<Result<ContextAlterationRequests>> Execute(SubscriptionWrapper subscriptionWrapper)
        => await Serialize(subscriptionWrapper).BindAsync(PrimeConnection).BindAsync(() => Result.Success(new ContextAlterationRequests(typeof(SubscriberReceiveContext))));

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