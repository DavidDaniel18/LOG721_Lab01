namespace SmallTransit.Application.Services.InfrastructureInterfaces;

public interface INetworkStreamCache
{
    void Add(INetworkStream networkStack);

    INetworkStream GetOrAdd(string key);
}