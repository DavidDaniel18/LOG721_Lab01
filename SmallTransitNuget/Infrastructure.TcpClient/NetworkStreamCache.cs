using System.Collections.Concurrent;
using Application.Services.InfrastructureInterfaces;

namespace Infrastructure.TcpClient;

public sealed class NetworkStreamCache : INetworkStreamCache
{
    private readonly ConcurrentDictionary<string, INetworkStream> _networkStacks = new();

    public void Add(INetworkStream networkStack)
    {
        _networkStacks.TryAdd(networkStack.Key, networkStack);
    }

    public INetworkStream Get(string key)
    {
        return _networkStacks[key];
    }
}