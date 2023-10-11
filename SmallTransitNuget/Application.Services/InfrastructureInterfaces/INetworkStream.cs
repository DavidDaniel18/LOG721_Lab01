using System.Net.Sockets;

namespace Application.Services.InfrastructureInterfaces;

public interface INetworkStream
{
    NetworkStream GetStream();
}