using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Receiving.States;

namespace SmallTransit.Domain.Services.Receiving.ClientReceive.States;

internal abstract class ClientReceiveState : ReceiveState<ClientReceiveContext, Protocol, ClientReceiveResult, ReceiveSendByteWrapper>
{
    protected ClientReceiveState(ClientReceiveContext context) : base(context)
    {
    }
}