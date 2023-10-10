using Domain.Common;

namespace Infrastructure.TcpClient;

internal sealed class ClientFactory
{
    internal static Result<INetworkStream> GetDestinationClient(string host, int port)
    {
        try
        {
            var destinationClient = new System.Net.Sockets.TcpClient(host, port);

            return Result.Success((INetworkStream)new NetworkClient(destinationClient));
        }
        catch (Exception)
        {
            return Result.Failure<INetworkStream>("Failed to connect to the server");
        }
    }
}