using Domain.ProtoTransit;
using Domain.Services.Receive.States;

namespace Domain.Services.Receive.ClientReceive.States;

internal abstract class ClientReceiveState : ReceiveState<ClientReceiveContext, Protocol, ClientReceiveResult, byte[]>
{
    protected ClientReceiveState(ClientReceiveContext context) : base(context)
    {
    }
}