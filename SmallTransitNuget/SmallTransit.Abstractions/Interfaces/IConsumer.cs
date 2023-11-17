namespace SmallTransit.Abstractions.Interfaces;

public interface IConsumer<in TContract> : IConsumer where TContract : class
{
    public Task Consume(TContract contract);
}

public interface IConsumer
{
}