using SmallTransit.Abstractions.Interfaces;

namespace SmallTransit.Abstractions.Configurator;

public interface IConfigurator
{
    void AddQueueConfigurator(Action<IQueueConfigurator> queueConfigurator);

    /// <summary>
    /// Type has to implement <see cref="IReceiver{TContract,TResult}"/>
    /// </summary>
    /// <param name="type"></param>
    void AddPointReceiver(Type type);

    void AddPointReceiver<TReceiver>() where TReceiver : class, IReceiver;

    void AddPointTarget(Action<ITargetConfiguration> pointConfigurator);

    int ExposedPortPoint { get; set; }
}