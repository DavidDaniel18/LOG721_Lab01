namespace SmallTransit.Abstractions.Configurator;

public interface ITargetConfiguration
{
    string TargetKey { get; set; }

    string Host { get; set; }

    int Port { get; set; }
}