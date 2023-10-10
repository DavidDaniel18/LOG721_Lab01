namespace Application.Common.Interfaces;

public interface IConsumer<in TContract> : IConsumer where TContract : class
{
    public Task Consume(TContract contract);
}

public interface IConsumer
{
}