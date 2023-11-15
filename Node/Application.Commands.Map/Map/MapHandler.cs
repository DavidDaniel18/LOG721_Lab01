using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using Domain.Services;
using SyncStore.Abstractions;

namespace Application.Commands.Map.Mapping;

public sealed class MapHandler : ICommandHandler<MapCommand>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<MapFinishedEvent> _publisher;

    public MapHandler(
        IMessagePublisher<MapFinishedEvent> publisher, 
        IHostInfo hostInfo)
    {
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public Task Handle(MapCommand command, CancellationToken cancellation)
    {
        Space space = command.space;
        List<Group> groups = command.groups;

        var groupId = GroupServices.GetClosestGroupByBarycentre(space, groups).Id;

        space.GroupId = groupId;

        _publisher.PublishAsync(new MapFinishedEvent(space, groups), _hostInfo.MapFinishedEventRoutingKey);

        return Task.CompletedTask;
    }
}