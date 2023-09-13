using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Configuration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Subscribe : ControllerBase
    {
        private readonly ILogger<Subscribe> _logger;

        public Subscribe(ILogger<Subscribe> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}