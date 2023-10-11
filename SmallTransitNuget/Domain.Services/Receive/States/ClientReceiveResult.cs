using Domain.ProtoTransit;

namespace Domain.Services.Receive.States;

internal sealed record ClientReceiveResult(Protocol Response) : StateResult<byte[]>(Response);