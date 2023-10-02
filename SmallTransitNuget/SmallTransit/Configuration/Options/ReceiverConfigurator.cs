namespace SmallTransit.Configuration.Options;

public sealed class ReceiverConfigurator
{
    public string RoutingKey { get; set; } = "*";

    public int PrefetchCount { get; set; } = 1;
}