using Application.Common.Interfaces;

namespace TestHost;

internal sealed class PublisherClient
{
    internal PublisherClient(IPublisher<WeatherForecast> publisher)
    {
        publisher.Publish(new WeatherForecast(), "john snow");
    }
}