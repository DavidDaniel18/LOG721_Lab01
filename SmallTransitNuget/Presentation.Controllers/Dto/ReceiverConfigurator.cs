using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Dto;

public sealed class ReceiverConfigurator : IReceiverConfigurator
{
    public string RoutingKey { get; set; } = "*";

    public int PrefetchCount { get; set; } = 1;
}