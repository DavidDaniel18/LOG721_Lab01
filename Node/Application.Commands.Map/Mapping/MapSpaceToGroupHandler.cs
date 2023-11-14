using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using Domain.Services;

namespace Application.Commands.Map.Mapping;

internal sealed class MapHandler : ICommandHandler<Map>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<Space> _publisher;

    // todo: link cache...
    private readonly List<Group> _groupsCache = new List<Group>();

    public MapHandler(IMessagePublisher<Space> publisher, IHostInfo hostInfo)
    {
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public Task Handle(Map command, CancellationToken cancellation)
    {
        Space space = command.space;

        var groupId = GroupServices.GetClosestGroupByBarycentre(space, _groupsCache).Id;
        space.GroupId = groupId;

        _publisher.PublishAsync(space, _hostInfo.MapFinishedEventRoutingKey);

        return Task.CompletedTask;
    }
}