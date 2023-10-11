using SmallTransit.Abstractions.Interfaces;
using TestReceiver;

namespace TestHost.Controllers;

internal sealed class ReceiverController : IConsumer<WeatherForecast>
{
    private readonly ILogger<ReceiverController> _logger;

    public ReceiverController(ILogger<ReceiverController> logger)
    {
        _logger = logger;
    }

    public Task Consume(WeatherForecast contract)
    {
        _logger.LogInformation("Received: {0}, consuming", contract.Summary);

        return Task.CompletedTask;
    }
}