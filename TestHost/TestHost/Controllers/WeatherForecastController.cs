using Microsoft.AspNetCore.Mvc;
using SmallTransit.Abstractions.Interfaces;

namespace TestHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IPublisher<WeatherForecast> _publisher;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IPublisher<WeatherForecast> publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult> Get()
        {
            var wfc = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(0)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };

            _logger.LogInformation("Publishing: {0}", wfc.Summary);

            await _publisher.Publish(wfc, "*");

            _logger.LogInformation("Published: {0}", wfc.Summary);

            return Ok(wfc);
        }
    }
}