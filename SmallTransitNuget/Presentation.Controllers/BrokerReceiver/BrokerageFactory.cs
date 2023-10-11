using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Broker;

namespace Presentation.Controllers.BrokerReceiver;

public sealed class BrokerageFactory : BrokerConnectionHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BrokerageFactory> _logger;

    public BrokerageFactory(IServiceProvider serviceProvider, ILogger<BrokerageFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        try
        {
            var handler = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<BrokerReceiver>();

            await handler.Receive(connection, CancellationTokenSource.CreateLinkedTokenSource(connection.ConnectionClosed));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Brokerage error");

            throw;
        }
        
    }
}