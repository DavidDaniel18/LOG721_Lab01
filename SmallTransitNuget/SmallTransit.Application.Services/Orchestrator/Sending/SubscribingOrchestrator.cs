using MessagePack;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Receiving.SubscriberReceive;
using SmallTransit.Domain.Services.Sending;
using SmallTransit.Domain.Services.Sending.Subscribing;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;

namespace SmallTransit.Application.Services.Orchestrator.Sending;

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