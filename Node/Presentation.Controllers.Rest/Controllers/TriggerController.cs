using Application.Commands.Map.Input;
using Application.Commands.Map.Mapping;
using Application.Common.Interfaces;
using Domain.Publicity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace Presentation.Controllers.Rest.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TriggerController : ControllerBase
    {
        private readonly IMessagePublisher<InputCommand> _inputPublisher;
        private readonly IMessagePublisher<MapCommand> _mapPublisher;
        private readonly IHostInfo _hostInfo;

        public TriggerController(
            IMessagePublisher<InputCommand> intputPublisher,
            IMessagePublisher<MapCommand> mapPublisher,
            IHostInfo hostInfo)
        {
            _hostInfo = hostInfo;
            _inputPublisher = intputPublisher;
            _mapPublisher = mapPublisher;
        }

        [HttpPost]
        [ActionName("Input")]
        public async Task<ActionResult> Input(bool Perpetual)
        {
            await _inputPublisher.PublishAsync(new InputCommand(_hostInfo.DataCsvName, _hostInfo.GroupCsvName), _hostInfo.InputRoutingKey);
            return Ok("Input sent.");
        }

        [HttpPost]
        [ActionName("Map")]
        public async Task<ActionResult> Map(string routingKey = "command/map1")
        {
            await _mapPublisher.PublishAsync(new MapCommand(new List<Space>()), routingKey);
            return Ok("Map sent.");
        }
    }
}