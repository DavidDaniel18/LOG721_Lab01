using System.Net.Sockets;

namespace Infrastructure.TcpClient;

public interface INetworkStream
{
    NetworkStream GetStream();
}