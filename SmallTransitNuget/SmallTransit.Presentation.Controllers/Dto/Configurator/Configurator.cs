using SmallTransit.Abstractions.Configurator;
using SmallTransit.Abstractions.Interfaces;

namespace SmallTransit.Presentation.Controllers.Dto.Configurator;

public sealed class Configurator : IConfigurator
{
    public readonly List<QueueConfiguratorService> QueueConfigurators = new();

    public readonly List<TargetConfiguration> TargetPointConfigurators = new();

    public readonly List<ReceiverPointConfiguration> ReceiverPointConfigurators = new();

    public int ExposedPortPoint { get; set; }

    public void AddQueueConfigurator(Action<IQueueConfigurator> queueConfigurator)
    {
        var configurator = new QueueConfiguratorService();

        queueConfigurator?.Invoke(configurator);

        configurator.TargetConfiguration!.Validate();

        QueueConfigurators.Add(configurator);
    }

    public void AddPointReceiver(Type type)
    {
        var implementedInterfaces = type.GetInterfaces();

        Type? consumerInterfaceType = null;

        foreach (var intf in implementedInterfaces)
        {
            if (intf.IsGenericType && intf.GetGenericTypeDefinition() == typeof(IReceiver<,>))
            {
                consumerInterfaceType = intf;

                var inArgument = consumerInterfaceType.GetGenericArguments()[0];
                var resultArgument = consumerInterfaceType.GetGenericArguments()[1];

                ReceiverPointConfigurators.Add(new ReceiverPointConfiguration(inArgument, resultArgument, consumerInterfaceType, type));

                break;
            }
        }

        if (consumerInterfaceType == null) throw new Exception("Consumer not found");
    }

    public void AddPointReceiver<TReceiver>() where TReceiver : class, IReceiver
    {
        var concreteConsumerType = typeof(TReceiver);

        AddPointReceiver(concreteConsumerType);
    }

    public void AddPointTarget(Action<ITargetConfiguration> pointConfigurator)
    {
        var configurator = new TargetConfiguration();

        pointConfigurator?.Invoke(configurator);

        configurator.Validate();

        TargetPointConfigurators.Add(configurator);
    }

}