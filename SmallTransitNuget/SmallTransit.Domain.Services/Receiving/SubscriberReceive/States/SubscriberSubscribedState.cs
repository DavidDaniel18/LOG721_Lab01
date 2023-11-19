using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.ProtoTransit.Entities.Messages.Core;
using SmallTransit.Domain.ProtoTransit.Entities.Messages.Data;
using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;
using SmallTransit.Domain.Services.Receiving.States;

namespace SmallTransit.Domain.Services.Receiving.SubscriberReceive.States;

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