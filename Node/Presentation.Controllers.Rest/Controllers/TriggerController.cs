using Application.Commands.Map.Input;
using Application.Commands.Map.Mapping;
using Application.Commands.Reducer.Reduce;
using Application.Common.Interfaces;
using Domain.Publicity;
using Domain.Grouping;
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
        private readonly IMessagePublisher<Reduce> _reducePublisher;
        private readonly IHostInfo _hostInfo;

        public TriggerController(
            IMessagePublisher<Reduce> reducePublisher,
            IMessagePublisher<InputCommand> intputPublisher,
            IMessagePublisher<MapCommand> mapPublisher,
            IHostInfo hostInfo)
        {
            _hostInfo = hostInfo;
            _inputPublisher = intputPublisher;
            _reducePublisher = reducePublisher;
            _mapPublisher = mapPublisher;
        }

        [HttpPost]
        [ActionName("Input")]
        public async Task<ActionResult> Input()
        {
            await _inputPublisher.PublishAsync(new InputCommand(_hostInfo.DataCsvName, _hostInfo.GroupCsvName), _hostInfo.InputRoutingKey);
            return Ok("Input sent.");
        }

        [HttpPost]
        [ActionName("Map")]
        public async Task<ActionResult> Map(string routingKey = "command/map1", int startIndex = 0, int endIndex = 24)
        {
            await _mapPublisher.PublishAsync(new MapCommand(startIndex, endIndex), routingKey);
            return Ok("Map sent.");
        }

        [HttpPost]
        [ActionName("Reduce")]
        public async Task<ActionResult> Reduce(string routingKey = "command/reduce1", string groupId = "#1", double barycentre = 88.1)
        {
            await _reducePublisher.PublishAsync(new Reduce(new Group(groupId, barycentre, ImmutableList<Space>.Empty)), routingKey);
            return Ok("Reduce sent.");
        }
    }
}