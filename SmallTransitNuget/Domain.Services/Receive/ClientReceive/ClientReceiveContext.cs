using Domain.ProtoTransit;
using Domain.Services.Receive.ClientReceive.States;
using Domain.Services.Receive.States;

namespace Domain.Services.Receive.ClientReceive;

internal sealed class ClientReceiveContext : ReceiveStateHolder<ClientReceiveContext, Protocol, ClientReceiveResult, byte[]>
{
    private protected override ReceiveState<ClientReceiveContext, Protocol, ClientReceiveResult, byte[]> State { get; set; }

    public ClientReceiveContext()
    {
        State = new ClientSubscribedState(this);
    }

    internal override bool GetConnectionReady()
    {
        throw new NotImplementedException();
    }
}