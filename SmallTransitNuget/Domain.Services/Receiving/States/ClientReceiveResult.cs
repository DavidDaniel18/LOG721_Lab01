using Domain.ProtoTransit;
using Domain.Services.Receiving.ClientReceive;

namespace Domain.Services.Receiving.States;

internal sealed record ClientReceiveResult(Protocol Response) : StateResult<ReceiveSendByteWrapper>(Response);