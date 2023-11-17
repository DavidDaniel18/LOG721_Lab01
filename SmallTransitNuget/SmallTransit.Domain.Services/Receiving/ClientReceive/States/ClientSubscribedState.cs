using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.ProtoTransit.Entities.Messages.Core;
using SmallTransit.Domain.ProtoTransit.Entities.Messages.Data;
using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;
using SmallTransit.Domain.Services.Receiving.States;

namespace SmallTransit.Domain.Services.Receiving.ClientReceive.States;

internal sealed class ClientSubscribedState : ClientReceiveState
{
    public ClientSubscribedState(ClientReceiveContext context) : base(context)
    {
    }

    internal override Result<ClientReceiveResult> Handle(Protocol message)
    {
        var ack = MessageFactory.Create(MessageTypesEnum.Send);

        switch (message)
        {
            case HandShake:
                return Result.Success(new ClientReceiveResult(ack));
            case Send send:
            {
                var senderId = send.TryGetProperty<SenderId>();
                var payloadType = send.TryGetProperty<PayloadType>();
                var payload = send.TryGetProperty<Payload>();

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