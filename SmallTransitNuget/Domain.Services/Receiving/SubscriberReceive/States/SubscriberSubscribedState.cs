using Domain.Common.Monads;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Entities.Messages.Core;
using Domain.ProtoTransit.Entities.Messages.Data;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Receiving.States;

namespace Domain.Services.Receiving.SubscriberReceive.States;

internal sealed class SubscriberSubscribedState : SubscriberReceiveState
{
    public SubscriberSubscribedState(SubscriberReceiveContext context) : base(context)
    {
    }

    internal override Result<SubscriberReceiveResult> Handle(Protocol message)
    {
        var ack = MessageFactory.Create(MessageTypesEnum.Ack);

        switch (message)
        {
            case Push pushMessage:
            {
                return pushMessage.TryGetProperty<Payload>().Bind(payload => Result.Success(new SubscriberReceiveResult(ack)
                {
                    Result = payload.Bytes
                }));
            }
            case Close:
                Context.SetState(new BrokerClosedState<SubscriberReceiveContext, SubscriberReceiveResult, byte[]>(Context));

                return Result.Success(new SubscriberReceiveResult(ack));
            default:
                return Result.Failure<SubscriberReceiveResult>("Unexpected message type");
        }
    }
}