using System.Net.Sockets;

namespace SmallTransit.Application.Services.InfrastructureInterfaces;

public interface INetworkStream 
{
    string Key { get; }

    NetworkStream GetStream();
}