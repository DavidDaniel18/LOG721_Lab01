using Domain.ProtoTransit;
using Domain.Services.Receive.BrokerReceive;

namespace Domain.Services.Receive.States;

internal sealed record BrokerReceiveResult(Protocol Response) : StateResult<ReceivePublishByteWrapper>(Response);