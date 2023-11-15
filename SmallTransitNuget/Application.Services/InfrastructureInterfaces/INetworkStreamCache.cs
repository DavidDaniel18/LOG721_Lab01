namespace Application.Services.InfrastructureInterfaces;

public interface INetworkStreamCache
{
    void Add(INetworkStream networkStack);

    INetworkStream Get(string key);
}