using Domain.ProtoTransit;
using Domain.Services.Receiving.BrokerReceive;

namespace Domain.Services.Receiving.States;

internal sealed record BrokerReceiveResult(Protocol Response) : StateResult<ReceivePublishByteWrapper>(Response);