﻿using Application.Commands.Map.Event;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Grouping;
using Domain.Publicity;
using Domain.Services;
using Microsoft.Extensions.Logging;
using SyncStore.Abstractions;

namespace Application.Commands.Map.Map;

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
        await Try.WithConsequenceAsync(async () =>
        {
            _logger.LogInformation($"Handler: {command.GetCommandName()}: Received");

            var spaceIdDic = command.SpaceIds.ToDictionary(s => s);

            var spacesFromCache = await _spaceCache.Query(s => s.Where(space => spaceIdDic.ContainsKey(space.Id)));

            var groups = await _groupsCache.Query(g => g);

            _logger.LogInformation($"Spaces ids count fetched from command: [{command.SpaceIds.Count()}]");

            spacesFromCache.ToList().ForEach(space => space.GroupId = GroupServices.GetClosestGroupByBarycentre(space, groups).Id);

            _logger.LogInformation("Saves spaces with closests group linked to it...");

            await _spaceCache.AddOrUpdateRange(spacesFromCache.Select(s => (s.Id, s)).ToList());

            await _spaceCache.SaveChangesAsync();

            _logger.LogInformation("Send Map terminated event...");

            return true;
        }, async (_, _) =>
        {
            await _spaceCache.UndoChangesAsync();

            _logger.LogInformation("Undo changes on spaces cache...");
        }, 
            retryCount: 5);
       
        _logger.LogInformation("Send MapFinishedEvent...");

        await _publisher.PublishAsync(new MapFinishedEvent(_hostInfo.MapRoutingKey), _hostInfo.MapFinishedEventRoutingKey);
    }
}