using System.Net.Sockets;
using Application.Services.InfrastructureInterfaces;

namespace Infrastructure.TcpClient;

internal sealed class NetworkClient : INetworkStream
{
    private readonly System.Net.Sockets.TcpClient _tcpClient;

    public NetworkClient(System.Net.Sockets.TcpClient tcpClient)
    {
        _tcpClient = tcpClient;
    }

    public NetworkStream GetStream()
    {
        return _tcpClient.GetStream();
    }
}