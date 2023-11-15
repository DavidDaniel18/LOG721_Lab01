using Domain.Common.Monads;
using Domain.ProtoTransit;
using Domain.ProtoTransit.Entities.Messages.Core;
using Domain.ProtoTransit.Entities.Messages.Data;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Receiving.States;

namespace Domain.Services.Receiving.ClientReceive.States;

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
                var senderId = pushMessage.TryGetProperty<SenderId>();
                var payloadType = pushMessage.TryGetProperty<PayloadType>();
                var payload = pushMessage.TryGetProperty<Payload>();

                if (Result.From(senderId, payloadType, payload) is { } combinedResult && combinedResult.IsFailure())
                {
                    return Result.FromFailure<ClientReceiveResult>(combinedResult);
                }

                return Result.Success(new ClientReceiveResult(ack)
                {
                    Result = new ReceiveSendByteWrapper(
                        senderId!.Content!.Bytes,
                        payloadType!.Content!.Bytes,
                        payload!.Content!.Bytes)
                });
                }
            case Close:
                Context.SetState(new BrokerClosedState<ClientReceiveContext, ClientReceiveResult, ReceiveSendByteWrapper>(Context));

                return Result.Success(new ClientReceiveResult(ack));
            default:
                return Result.Failure<ClientReceiveResult>("Unexpected message type");
        }
    }
}