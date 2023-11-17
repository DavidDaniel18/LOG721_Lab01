using SmallTransit.Abstractions.Receiver;

namespace SmallTransit.Abstractions.Interfaces;

public interface IReceiver<TContract, TResult> : IReceiver 
where TContract : class
where TResult : notnull
{
    public Task<TResult> Consume(ReceiveContext<TContract> context);
}

public interface IReceiver
{
}