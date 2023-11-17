﻿using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.Map.Input;

namespace Presentation.Controllers.Rest.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class TriggerController : ControllerBase
    {
        private readonly IMessagePublisher<InputCommand> _publisher;
        private readonly IHostInfo _hostInfo;

        public TriggerController(IMessagePublisher<InputCommand> publisher, IHostInfo hostInfo)
        {
            _hostInfo = hostInfo;
            _publisher = publisher;
        }

        [HttpPost]
        [ActionName("Post")]
        public async Task<ActionResult> Post(int nbrIteration)
        {
            await _publisher.PublishAsync(new InputCommand(_hostInfo.DataCsvName, _hostInfo.GroupCsvName), _hostInfo.InputRoutingKey);
            return Ok("Started");
        }
    }
}
