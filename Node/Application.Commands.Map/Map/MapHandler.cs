using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using Domain.Services;

namespace Application.Commands.Map.Mapping;

public sealed class MapHandler : ICommandHandler<MapCommand>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<MapFinishedEvent> _publisher;

    // todo: link cache...
    private readonly List<Group> _groupsCache = new List<Group>();

    public MapHandler(IMessagePublisher<MapFinishedEvent> publisher, IHostInfo hostInfo)
    {
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public Task Handle(MapCommand command, CancellationToken cancellation)
    {
        Space space = command.space;

        var groupId = GroupServices.GetClosestGroupByBarycentre(space, _groupsCache).Id;
        space.GroupId = groupId;

        _publisher.PublishAsync(new MapFinishedEvent(space), _hostInfo.MapFinishedEventRoutingKey);

        return Task.CompletedTask;
    }
}