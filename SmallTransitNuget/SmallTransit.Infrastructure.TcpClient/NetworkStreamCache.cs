using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Configurator;
using SmallTransit.Application.Services.InfrastructureInterfaces;

namespace SmallTransit.Infrastructure.TcpClient;

public sealed class NetworkStreamCache : INetworkStreamCache
{
    private readonly ConcurrentDictionary<string, ITargetConfiguration> _configurations;
    private readonly ClientFactory _clientFactory;

    private readonly ConcurrentDictionary<string, INetworkStream> _networkStacks = new();

    public NetworkStreamCache(ConcurrentDictionary<string, ITargetConfiguration> configurations, ClientFactory clientFactory)
    {
        _configurations = configurations;
        _clientFactory = clientFactory;
    }

    public INetworkStream GetOrAdd(string key, ILogger logger)
    {
        logger.LogInformation("creating bus to, key: {key}", key);

        return _networkStacks.GetOrAdd(key, key =>
        {
            try
            {
                logger.LogInformation("creating bus to, key: {key}", key);

                var configuration = _configurations[key];

                var result = _clientFactory.RetryCreateClient(configuration.Host, configuration.Port, configuration.TargetKey);

                return result.GetValueOrThrow();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error creating network stream");

                throw;
            }
           
        });
    }
}