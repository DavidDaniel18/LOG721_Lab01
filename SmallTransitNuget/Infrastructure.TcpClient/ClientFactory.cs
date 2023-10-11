using Application.Services.InfrastructureInterfaces;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Infrastructure.TcpClient;

public sealed class ClientFactory
{
    private readonly ILogger<ClientFactory> _logger;

    public ClientFactory(ILogger<ClientFactory> logger)
    {
        _logger = logger;
    }

    public Result<INetworkStream> RetryCreateClient(string host, int port)
    {
        do
        {
            var connectionResult = GetDestinationClient(host, port);

            if (connectionResult.IsFailure())
            {
                Task.Delay(1000).Wait();

                continue;
            }

            _logger.LogInformation("bus created");

            return connectionResult;

        } while (true);
    }

    private Result<INetworkStream> GetDestinationClient(string host, int port)
    {
        try
        {
            var destinationClient = new System.Net.Sockets.TcpClient(host, port);

            return Result.Success((INetworkStream)new NetworkClient(destinationClient));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Client creation error");

            return Result.Failure<INetworkStream>(e);
        }
    }
}