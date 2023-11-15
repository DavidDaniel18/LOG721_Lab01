using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Receiver;

namespace Presentation.Controllers.Client;

public sealed class ReceiverFactory : ReceiverConnectionHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ReceiverFactory> _logger;

    public ReceiverFactory(IServiceProvider serviceProvider, ILogger<ReceiverFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        try
        {
            var handler = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Receiver>();

            var result = await handler.Receive(connection, CancellationTokenSource.CreateLinkedTokenSource(connection.ConnectionClosed));

            if (result.IsFailure())
            {
                _logger.LogError(result.Exception!.Message);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Receiver error");

            throw;
        }
        
    }
}