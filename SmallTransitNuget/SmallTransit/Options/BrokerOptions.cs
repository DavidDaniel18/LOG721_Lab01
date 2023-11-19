using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Configurator;
using SmallTransit.Abstractions.Interfaces;

namespace SmallTransit.Options;

internal sealed class BrokerOptions : ISBrokerConfigurator
{
    public int TcpPort { get; set; }

    internal Type SubscriptionReceiver { get; set; }

    public Type PublishReceive { get; set; }

    public void AddSubscriptionReceiver<TConsumer>() where TConsumer : class, IConsumer<BrokerSubscriptionDto>
    {
        SubscriptionReceiver = typeof(TConsumer);
    }

    public void AddPublishReceiver<TConsumer>() where TConsumer : class, IConsumer<BrokerReceiveWrapper>
    {
        PublishReceive = typeof(TConsumer);
    }
}