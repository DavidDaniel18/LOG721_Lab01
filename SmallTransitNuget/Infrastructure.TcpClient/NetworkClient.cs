using System.Net.Sockets;

namespace Infrastructure.TcpClient;

internal sealed class NetworkClient : INetworkStream
{
    private readonly System.Net.Sockets.TcpClient _tcpClient;

    internal NetworkClient(System.Net.Sockets.TcpClient tcpClient)
    {
        _tcpClient = tcpClient;
    }
    public NetworkStream GetStream()
    {
        return _tcpClient.GetStream();
    }
}