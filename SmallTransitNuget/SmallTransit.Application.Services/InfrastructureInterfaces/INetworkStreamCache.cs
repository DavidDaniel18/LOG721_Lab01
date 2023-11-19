using Microsoft.Extensions.Logging;

namespace SmallTransit.Application.Services.InfrastructureInterfaces;

public interface INetworkStreamCache
{
    INetworkStream GetOrAdd(string key, ILogger logger);
}