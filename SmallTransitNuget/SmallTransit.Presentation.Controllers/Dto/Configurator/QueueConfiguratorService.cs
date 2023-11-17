using System.Collections.Concurrent;
using SmallTransit.Abstractions.Configurator;
using SmallTransit.Abstractions.Interfaces;

namespace SmallTransit.Presentation.Controllers.Dto.Configurator;

public sealed class QueueConfiguratorService : IQueueConfigurator
{
    public TargetConfiguration TargetConfiguration { get; private set; }
 
    public readonly ConcurrentBag<ReceiverConfiguration> ReceiverConfigurator = new();

    public void Host(string brokerKey, string host, int port)
    {
        TargetConfiguration = new TargetConfiguration()
        {
            Host = host,
            Port = port,
            TargetKey = brokerKey
        };

        TargetConfiguration.Validate();
    }

    public void AddReceiver<TConsumer>(string queueName, Action<IReceiverConfigurator> configure) where TConsumer : class, IConsumer
    {
        var configurator = new ReceiverConfigurator();

        configure.Invoke(configurator);

        var concreteConsumerType = typeof(TConsumer);

        var implementedInterfaces = typeof(TConsumer).GetInterfaces();

        Type? consumerInterfaceType = null;

        foreach (var intf in implementedInterfaces)
        {
            if (intf.IsGenericType && intf.GetGenericTypeDefinition() == typeof(IConsumer<>))
            {
                consumerInterfaceType = intf;

                var genericArgument = consumerInterfaceType.GetGenericArguments()[0];

                ReceiverConfigurator.Add(new ReceiverConfiguration(genericArgument, consumerInterfaceType, concreteConsumerType, queueName, configurator));

                break;
            }
        }

        if (consumerInterfaceType == null) throw new Exception("Consumer not found");
    }
}