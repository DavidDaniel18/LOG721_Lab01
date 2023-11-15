using SmallTransit.Abstractions.Interfaces;

namespace SmallTransit.Abstractions.Configurator;

public interface IQueueConfigurator
{
    void Host(string brokerKey, string host, int port);

    void AddReceiver<TConsumer>(string queueName, Action<IReceiverConfigurator> configure) where TConsumer : class, IConsumer;
}