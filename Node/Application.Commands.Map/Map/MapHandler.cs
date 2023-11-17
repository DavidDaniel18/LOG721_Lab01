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

    private readonly ISyncStore<string, Group> _groupsCache;

    private readonly ISyncStore<string, Space> _spaceCache;

    public MapHandler(
        ISyncStore<string, Group> groupSyncStore, 
        ISyncStore<string, Space> spaceSyncStore, 
        IMessagePublisher<MapFinishedEvent> publisher, 
        IHostInfo hostInfo)
    {
        _hostInfo = hostInfo;
        _publisher = publisher;
        _groupsCache = groupSyncStore;
        _spaceCache = spaceSyncStore;
    }

    public async Task Handle(MapCommand command, CancellationToken cancellation)
    {
        var groups = await _groupsCache.Query(g => g);
        
        command.spaces.ForEach(space => GroupServices.GetClosestGroupByBarycentre(space, groups).Spaces.Add(space));

        await _groupsCache.AddOrUpdateRange(groups.Select(g => (g.Id, g)));
        await _spaceCache.SaveChangesAsync();

        await _publisher.PublishAsync(new MapFinishedEvent(_hostInfo.MapRoutingKey), _hostInfo.MapFinishedEventRoutingKey);
    }
}