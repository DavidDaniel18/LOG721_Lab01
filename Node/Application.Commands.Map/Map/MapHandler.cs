using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using Domain.Services;
using Microsoft.Extensions.Logging;
using SyncStore.Abstractions;

namespace Application.Commands.Map.Mapping;

public sealed class MapHandler : ICommandHandler<MapCommand>
{
    private readonly ILogger<MapHandler> _logger;

    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<MapFinishedEvent> _publisher;

    private readonly ISyncStore<string, Group> _groupsCache;

    private readonly ISyncStore<string, Space> _spaceCache;

    public MapHandler(
        ILogger<MapHandler> logger,
        ISyncStore<string, Group> groupSyncStore, 
        ISyncStore<string, Space> spaceSyncStore, 
        IMessagePublisher<MapFinishedEvent> publisher, 
        IHostInfo hostInfo)
    {
        _logger = logger;
        _hostInfo = hostInfo;
        _publisher = publisher;
        _groupsCache = groupSyncStore;
        _spaceCache = spaceSyncStore;
    }

    public async Task Handle(MapCommand command, CancellationToken cancellation)
    {
        _logger.LogInformation($"Handler: {command.GetCommandName()}: Received");
        var groups = await _groupsCache.Query(g => g);
        
        command.spaces.ForEach(space => GroupServices.GetClosestGroupByBarycentre(space, groups).Spaces.Add(space));

        _logger.LogInformation("Saves groups with closests spaces linked to it...");
        await _groupsCache.AddOrUpdateRange(groups.Select(g => (g.Id, g)));
        await _spaceCache.SaveChangesAsync();

        _logger.LogInformation("Send Map terminated event...");
        await _publisher.PublishAsync(new MapFinishedEvent(_hostInfo.MapRoutingKey), _hostInfo.MapFinishedEventRoutingKey);
    }
}