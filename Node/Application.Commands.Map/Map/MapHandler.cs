using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using DomainNode.Services;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Map.Mapping;

public sealed class MapHandler : ICommandHandler<MapCommand>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<MapFinishedEvent> _publisher;

    private readonly ILogger<MapHandler> _logger;

    public MapHandler(
        ILogger<MapHandler> logger,
        IMessagePublisher<MapFinishedEvent> publisher, 
        IHostInfo hostInfo)
    {
        _logger = logger;
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public Task Handle(MapCommand command, CancellationToken cancellation)
    {
        _logger.LogInformation($"MapHandler(): {command.space.Id}");

        Space space = command.space;
        List<Group> groups = command.groups;

        _logger.LogInformation($"Find closest group...");
        var groupId = GroupServices.GetClosestGroupByBarycentre(space, groups).Id;
        _logger.LogInformation($"Found closest group {groupId}");

        space.GroupId = groupId;

        _logger.LogInformation($"Send MapFinishedEvent");

        Task.WaitAll(_publisher.PublishAsync(new MapFinishedEvent(space, groups), _hostInfo.MapFinishedEventRoutingKey));

        _logger.LogInformation($"MapHandler Completed");

        return Task.CompletedTask;
    }
}