using SmallTransit.Presentation.Controllers.Dto.Configurator;

namespace SmallTransit.Presentation.Controllers.Dto;

public sealed class ConfiguratorCollection
{
    public IReadOnlyCollection<QueueConfiguratorService> Configurators { get; }

    public ConfiguratorCollection(IReadOnlyCollection<QueueConfiguratorService> configurators)
    {
        Configurators = configurators;
    }
}