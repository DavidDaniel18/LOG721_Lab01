using SmallTransit.Domain.ProtoTransit;
using SmallTransit.Domain.Services.Receiving.BrokerReceive;

namespace SmallTransit.Domain.Services.Receiving.States;

internal sealed record BrokerReceiveResult(Protocol Response) : StateResult<ReceivePublishByteWrapper>(Response);