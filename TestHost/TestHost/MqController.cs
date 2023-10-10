using Application.Common.Interfaces;

namespace TestHost;

internal sealed class MqController : IConsumer<WeatherForecast>
{
    public Task Consume(WeatherForecast contract)
    {
        throw new NotImplementedException();
    }
}