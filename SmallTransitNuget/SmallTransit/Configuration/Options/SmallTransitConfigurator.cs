using System.Collections.Concurrent;

namespace SmallTransit.Configuration.Options;

public sealed class SmallTransitConfigurator
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 0;

    internal readonly ConcurrentBag<ReceiverConfiguration> ReceiverConfigurator = new ();

    public void AddReceiver<TContract>(string queueName, Action<ReceiverConfigurator> configure) where TContract : class
    {
        var configurator = new ReceiverConfigurator();

        configure.Invoke(configurator);

        ReceiverConfigurator.Add(new ReceiverConfiguration(typeof(TContract), queueName, configurator));

    }

    internal sealed class ReceiverConfiguration
    {
        internal Type ContractType { get; }

        internal string QueueName { get; }

        internal ReceiverConfigurator ReceiverConfigurator { get; }

        public ReceiverConfiguration(Type contractType, string queueName, ReceiverConfigurator receiverConfigurator)
        {
            ContractType = contractType;
            QueueName = queueName;
            ReceiverConfigurator = receiverConfigurator;
        }
    }
}