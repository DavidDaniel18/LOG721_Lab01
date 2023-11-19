using SmallTransit.Abstractions.Interfaces;

namespace SmallTransit.Presentation.Controllers.Dto.Configurator;

public sealed class ReceiverConfigurator : IReceiverConfigurator
{
    public string RoutingKey { get; set; } = "*";

    public int PrefetchCount { get; set; } = 1;
}