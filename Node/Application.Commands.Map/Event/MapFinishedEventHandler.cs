﻿using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Grouping;
using Domain.Publicity;
using Application.Commands.Orchestrator.Shuffle;

namespace Application.Commands.Map.Event;

public sealed class MapFinishedEventHandler : ICommandHandler<MapFinishedEvent>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<Shuffle> _publisher;

    private ISingletonCache<Space> _spaceCache;
    private ISingletonCache<Group> _groupCache;
    private ISingletonCache<MapFinishedBool> _spaceFinishedCache;

    public MapFinishedEventHandler(
        ISingletonCache<Space> spaceCache,
        ISingletonCache<Group> groupCache,
        ISingletonCache<MapFinishedBool> spaceFinishedCache,
        IHostInfo hostInfo, 
        IMessagePublisher<Shuffle> publisher)
    {
        _spaceCache = spaceCache;
        _groupCache = groupCache;
        _spaceFinishedCache = spaceFinishedCache;
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public Task Handle(MapFinishedEvent command, CancellationToken cancellation)
    {
        _spaceFinishedCache.Value.TryAdd(command.space.Id, new MapFinishedBool { Value = true, Id = command.space.Id });

        // Add space to groups cache.
        if (_groupCache.Value.TryGetValue(command.space.GroupId ?? "", out var group))
        {
            var groupSpaces = group.Spaces;
            groupSpaces.Add(command.space);

            _groupCache.Value.TryUpdate(group.Id, new Group(group.Id, group.Barycentre, groupSpaces), group);
        }

        var spaces = _spaceCache.Value.Values;
        var result = _spaceFinishedCache.Value.Values;

        bool isFinished = result.All(s => s.Value) && result.Count() == spaces.Count();

        if (isFinished)
        {
            _publisher.PublishAsync(new Shuffle(), _hostInfo.MapShuffleRoutingKey);
        }

        return Task.CompletedTask;
    }
}