using Presentation.Controllers.Dto.Configurator;

namespace Presentation.Controllers.Dto;

public sealed class ConfiguratorCollection
{
    public IReadOnlyCollection<QueueConfiguratorService> Configurators { get; }

    public ConfiguratorCollection(IReadOnlyCollection<QueueConfiguratorService> configurators)
    {
        Configurators = configurators;
    }
}