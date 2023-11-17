using System.Net.Sockets;
using SmallTransit.Application.Services.InfrastructureInterfaces;

namespace SmallTransit.Infrastructure.TcpClient;

internal sealed class NetworkClient : INetworkStream
{
    public string Key { get; }

    private readonly System.Net.Sockets.TcpClient _tcpClient;

    public NetworkClient(System.Net.Sockets.TcpClient tcpClient, string key)
    {
        _tcpClient = tcpClient;
        Key = key;
    }

    public NetworkStream GetStream()
    {
        return _tcpClient.GetStream();
    }
}