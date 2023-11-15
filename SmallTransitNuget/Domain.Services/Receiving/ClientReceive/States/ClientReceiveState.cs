using Domain.ProtoTransit;
using Domain.Services.Receiving.States;

namespace Domain.Services.Receiving.ClientReceive.States;

internal abstract class ClientReceiveState : ReceiveState<ClientReceiveContext, Protocol, ClientReceiveResult, ReceiveSendByteWrapper>
{
    protected ClientReceiveState(ClientReceiveContext context) : base(context)
    {
    }
}