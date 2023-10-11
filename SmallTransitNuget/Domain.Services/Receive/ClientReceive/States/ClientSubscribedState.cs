using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Entities.Messages.Core;
using Domain.ProtoTransit.Entities.Messages.Data;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Receive.States;

namespace Domain.Services.Receive.ClientReceive.States;

internal sealed class ClientSubscribedState : ClientReceiveState
{
    public ClientSubscribedState(ClientReceiveContext context) : base(context)
    {
    }

    internal override Result<ClientReceiveResult> Handle(Protocol message)
    {
        var ack = MessageFactory.Create(MessageTypesEnum.Ack);

        switch (message)
        {
            case Push pushMessage:
            {
                return pushMessage.TryGetProperty<Payload>().Bind(payload => Result.Success(new ClientReceiveResult(ack)
                {
                    Result = payload.Bytes
                }));
            }
            case Close:
                Context.SetState(new BrokerClosedState<ClientReceiveContext, ClientReceiveResult, byte[]>(Context));

                return Result.Success(new ClientReceiveResult(ack));
            default:
                return Result.Failure<ClientReceiveResult>("Unexpected message type");
        }
    }
}