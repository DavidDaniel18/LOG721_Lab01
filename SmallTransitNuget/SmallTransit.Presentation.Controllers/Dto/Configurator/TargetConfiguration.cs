using SmallTransit.Abstractions.Configurator;

namespace SmallTransit.Presentation.Controllers.Dto.Configurator;

public sealed class TargetConfiguration : ITargetConfiguration
{
    public string TargetKey { get; set; }

    public string Host { get; set; }

    public int Port { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(TargetKey))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(TargetKey));
        }

        if (string.IsNullOrWhiteSpace(Host))
        {
            Host = "host.docker.internal";
        }

        if (Port <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Port));
        }
    }
}