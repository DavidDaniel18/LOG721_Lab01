using System.Collections.Concurrent;
using Application.Common.Interfaces;

namespace Configuration.Options;

public sealed class SmallTransitConfigurator
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 0;

    internal readonly ConcurrentBag<ReceiverConfiguration> ReceiverConfigurator = new ();

    public void AddReceiver<TConsumer>(string queueName, Action<ReceiverConfigurator> configure) where TConsumer : class, IConsumer
    {
        var configurator = new ReceiverConfigurator();

        configure.Invoke(configurator);

        ReceiverConfigurator.Add(new ReceiverConfiguration(typeof(TConsumer), queueName, configurator));
    }
}