using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Receiving.ClientReceive;

namespace SmallTransit.Domain.Services.Receiving.States;

internal sealed record ClientReceiveResult(Protocol Response) : StateResult<ReceiveSendByteWrapper>(Response);