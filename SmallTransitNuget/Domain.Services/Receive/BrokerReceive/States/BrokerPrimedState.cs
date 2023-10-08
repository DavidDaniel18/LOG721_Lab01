using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Entities.Messages.Core;
using Domain.ProtoTransit.Entities.Messages.Data;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Receive.States;

namespace Domain.Services.Receive.BrokerReceive.States;

internal sealed class BrokerPrimedState : BrokerReceiveState
{
    public BrokerPrimedState(BrokerReceiveContext context) : base(context)
    {
    }

    internal override Result<BrokerReceiveResult> Handle(Protocol message)
    {
        var ack = MessageFactory.Create(MessageTypesEnum.Ack);

        switch (message)
        {
            case Publish publishMessage:
            {
                var routingKey = publishMessage.TryGetProperty<RoutingKey>();
                var payloadType = publishMessage.TryGetProperty<PayloadType>();
                var payload = publishMessage.TryGetProperty<Payload>();

                if (Result.From(routingKey, payloadType, payload) is { } combinedResult && combinedResult.IsFailure())
                {
                    return Result.FromFailure<BrokerReceiveResult>(combinedResult);
                }

                return Result.Success(new BrokerReceiveResult(ack)
                {
                    Result = new ReceivePublishByteWrapper(
                        routingKey!.Content!.Bytes,
                        payloadType!.Content!.Bytes,
                        payload!.Content!.Bytes)
                });
            }
            case Close:
                Context.SetState(new BrokerClosedState(Context));

                return Result.Success(new BrokerReceiveResult(ack));
            default:
                return Result.Failure<BrokerReceiveResult>("Unexpected message type");
        }
    }
}