namespace SmallTransit.Abstractions.Receiver;

public sealed record ReceiveContext<TContract>(string SenderId, TContract Contract);