using Domain.ProtoTransit;

namespace Domain.Services.Receiving.States;

internal sealed record SubscriberReceiveResult(Protocol Response) : StateResult<byte[]>(Response);