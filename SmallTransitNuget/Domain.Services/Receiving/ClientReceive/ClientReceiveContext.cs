using Domain.ProtoTransit;
using Domain.ProtoTransit.Entities.Messages.Data;
using Domain.ProtoTransit.ValueObjects.Properties;
using Domain.Services.Receiving.ClientReceive.States;
using Domain.Services.Receiving.States;

namespace Domain.Services.Receiving.ClientReceive;

internal sealed class ClientReceiveContext : ReceiveStateHolder<ClientReceiveContext, Protocol, ClientReceiveResult, ReceiveSendByteWrapper>
{
    private protected override ReceiveState<ClientReceiveContext, Protocol, ClientReceiveResult, ReceiveSendByteWrapper> State { get; set; }

    public ClientReceiveContext()
    {
        State = new ClientSubscribedState(this);
    }

    internal Protocol GetPayloadResponse(byte[] payload, byte[] payloadType)
    {
        var protocol = (PayloadResponse)MessageFactory.Create(MessageTypesEnum.PayloadResponse);

        protocol.TrySetProperty<Payload>(payload);
        protocol.TrySetProperty<PayloadType>(payloadType);

        return protocol;
    }

    internal override bool GetConnectionReady()
    {
        throw new NotImplementedException();
    }
}