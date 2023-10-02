using Controlleur;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Configuration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Subscribe : ControllerBase
    {
        private readonly ILogger<Subscribe> _logger;
        private readonly IJohny _johny;

        public Subscribe(ILogger<Subscribe> logger, IJohny johny)
        {
            _logger = logger;
            _johny = johny;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public ActionResult Get()
        {
            return Ok(_johny.GetMessage());
        }
    }
}