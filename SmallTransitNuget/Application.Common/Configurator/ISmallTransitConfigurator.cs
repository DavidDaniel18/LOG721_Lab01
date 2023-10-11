using SmallTransit.Abstractions.Interfaces;

namespace SmallTransit.Abstractions.Configurator;

public interface ISmallTransitConfigurator
{
    string Host { get; set; }
    int Port { get; set; }
    void AddReceiver<TConsumer>(string queueName, Action<IReceiverConfigurator> configure) where TConsumer : class, IConsumer;
}