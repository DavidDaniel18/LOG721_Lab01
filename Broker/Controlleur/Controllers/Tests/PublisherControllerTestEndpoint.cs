using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Configuration.Controllers;
using Interfaces.Domain;
using Interfaces.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Controllers.Controllers.Tests
{
    [ApiController]
    [Route("[controller]")]
    public class PublisherControllerTestEndpoint : ControllerBase
    {
        private readonly ILogger<PublisherControllerTestEndpoint> _logger;
        private readonly IPublisherHandler _publisherHandler;

        public PublisherControllerTestEndpoint(ILogger<PublisherControllerTestEndpoint> logger, IPublisherHandler publisherHandler)
        {
            _publisherHandler = publisherHandler;
            _logger = logger;
        }

        [HttpGet("advertise")]
        public IActionResult Advertise()
        {
            _logger.LogInformation("Advertise called");
            //_publisherHandler.Advertise("todo_route");
            return Ok("Advertise");
        }

        [HttpGet("unadvertise")]
        public IActionResult UnAdvertise()
        {
            _logger.LogInformation("UnAdvertise called");
            //_publisherHandler.UnAdvertise("todo_route");
            return Ok("UnAdvertise");
        }

        [HttpGet("publish")]
        public IActionResult Publish()
        {
            _logger.LogInformation("Publish called");
            //_publisherHandler.Publish(publication);
            return Ok("Publish");
        }
    }
}
