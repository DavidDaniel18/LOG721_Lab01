using SmallTransit.Domain.ProtoTransit;

namespace SmallTransit.Domain.Services.Receiving.States;

internal sealed record SubscriberReceiveResult(Protocol Response) : StateResult<byte[]>(Response);