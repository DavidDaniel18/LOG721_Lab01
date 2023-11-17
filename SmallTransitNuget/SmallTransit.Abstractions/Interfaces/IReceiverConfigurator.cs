namespace SmallTransit.Abstractions.Interfaces;

public interface IReceiverConfigurator
{
    string RoutingKey { get; set; }

    int PrefetchCount { get; set; }
}