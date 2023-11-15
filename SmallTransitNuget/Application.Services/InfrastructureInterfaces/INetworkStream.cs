using System.Net.Sockets;

namespace Application.Services.InfrastructureInterfaces;

public interface INetworkStream
{
    string Key { get; }

    NetworkStream GetStream();
}