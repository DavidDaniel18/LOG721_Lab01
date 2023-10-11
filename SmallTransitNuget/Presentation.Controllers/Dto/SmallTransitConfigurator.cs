using System.Collections.Concurrent;
using SmallTransit.Abstractions.Configurator;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Dto;

public sealed class SmallTransitConfigurator : ISmallTransitConfigurator
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 0;

    public readonly ConcurrentBag<ReceiverConfiguration> ReceiverConfigurator = new();

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