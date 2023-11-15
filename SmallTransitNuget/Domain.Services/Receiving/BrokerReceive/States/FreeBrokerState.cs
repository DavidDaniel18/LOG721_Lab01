using Domain.Common.Monads;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Entities.Messages.Core;
using Domain.ProtoTransit.Entities.Messages.Data;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Receiving.States;
using Domain.Services.Sending.Subscribing.Dto;

namespace Domain.Services.Receiving.BrokerReceive.States;

internal sealed class FreeBrokerState : BrokerReceiveState
{
    public FreeBrokerState(BrokerReceiveContext context) : base(context) { }

    internal override Result<BrokerReceiveResult> Handle(Protocol payload)
    {
        switch (payload)
        {
            case HandShake:
                {
                    var returnProtocol = MessageFactory.Create(MessageTypesEnum.Ack);

                    Context.SetState(new BrokerPrimedState(Context));

                    return Result.Success(new BrokerReceiveResult(returnProtocol));
                }
            case Subscribe:
                {
                    var returnProtocol = MessageFactory.Create(MessageTypesEnum.Ack);

                    Context.SetState(new BrokerSubscribedState(Context));

                    var routingKeyResult = payload.TryGetProperty<RoutingKey>();
                    var payloadTypeResult = payload.TryGetProperty<PayloadType>();
                    var queueNameResult = payload.TryGetProperty<QueueName>();

                    return Result.From(routingKeyResult, payloadTypeResult, queueNameResult)
                        .Bind(() => Result.Success(new BrokerReceiveResult(returnProtocol)
                        {
                            SubscriptionDto = new SubscriptionDto(
                                routingKeyResult.Content!.Bytes,
                                payloadTypeResult.Content!.Bytes,
                                queueNameResult.Content!.Bytes)
                        }));
                }
            default:
                return Result.Failure<BrokerReceiveResult>("Unexpected message type");
        }
    }
}