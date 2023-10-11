using Configuration.Controllers;
using Entities;
using Interfaces.Handler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Controllers.Tests
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriberControllerTestEndpoint : ControllerBase
    {
        private readonly ILogger<SubscriberControllerTestEndpoint> _logger;
        private readonly ISubscriptionHandler _subscriptionHandler;

        public SubscriberControllerTestEndpoint(ILogger<SubscriberControllerTestEndpoint> logger, ISubscriptionHandler subscriptionHandler)
        {
            _subscriptionHandler = subscriptionHandler;
            _logger = logger;
        }

        [HttpGet("subscribe")]
        public IActionResult Subscribe()
        {
            _logger.LogInformation("Subscribe called");
            //_subscriptionHandler.Subscribe(new Subscription());
            return Ok("Subscribe");
        }

        [HttpGet("unsubscribe")]
        public IActionResult UnSubscribe()
        {
            _logger.LogInformation("UnSubscribe called");
            //_subscriptionHandler.Unsubscribe(new Subscription());
            return Ok("UnSubscribe");
        }

        [HttpGet("listen")]
        public IActionResult Listen()
        {
            _logger.LogInformation("Listen called");
            //_subscriptionHandler.Listen(new Subscription());
            return Ok("Listen");
        }
    }
}
