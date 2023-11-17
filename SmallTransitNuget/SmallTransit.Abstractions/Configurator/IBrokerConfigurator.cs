using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Interfaces;

namespace SmallTransit.Abstractions.Configurator;

public interface ISBrokerConfigurator
{
    int TcpPort { get; set; }

    void AddSubscriptionReceiver<TConsumer>() where TConsumer : class, IConsumer<BrokerSubscriptionDto>;

    void AddPublishReceiver<TConsumer>() where TConsumer : class, IConsumer<BrokerReceiveWrapper>;
}