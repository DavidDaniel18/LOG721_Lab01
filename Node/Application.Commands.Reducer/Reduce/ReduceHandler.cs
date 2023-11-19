using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using Microsoft.Extensions.Logging;
using SyncStore.Abstractions;
using System.Collections.Immutable;
using Application.Commands.Reducer.Event;

namespace Application.Commands.Reducer.Reduce;

public sealed class ReduceHandler : ICommandHandler<Commands.Reduce>
{
    private readonly ILogger<ReduceHandler> _logger;

    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<ReduceFinishedEvent> _publisher;

    private ISyncStore<string, Group> _groupsCache;

    private ISyncStore<string, Space> _spacesCache;

    public ReduceHandler(
        ILogger<ReduceHandler> logger,
        ISyncStore<string, Group> groupsCache,
        ISyncStore<string, Space> spaceCache,
        IHostInfo hostInfo, 
        IMessagePublisher<ReduceFinishedEvent> publisher)
    {
        _spacesCache = spaceCache;
        _logger = logger;
        _groupsCache = groupsCache;
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public async Task Handle(Commands.Reduce command, CancellationToken cancellation)
    {
        _logger.LogInformation($"Handler: {command.GetCommandName()}: Received");
        
        var spaces = await _spacesCache.Query(spaces => spaces.Where(space => space.GroupId!.Equals(command.group.Id)));

        var group = await _groupsCache.TryGet(command.group.Id);

        if (group == null)
            throw new Exception($"Did not find group {command.group.Id}");

        _logger.LogInformation($"Calculate avg for groupId: {command.group.Id}");

        double newBarycentre = 0;

        if (spaces.Any())
        {
            newBarycentre = spaces.Average(space => space.GetNormalizedValue());
        }

        _logger.LogInformation($"Save avg {newBarycentre} and emptied the spaces list of the group...");
        var newGroup = new Group(command.group.Id, newBarycentre, ImmutableList<Space>.Empty);

        await _groupsCache.AddOrUpdate(command.group.Id, newGroup);
        await _groupsCache.SaveChangesAsync();

        var delta = Math.Abs(newBarycentre - group.Barycentre);

        _logger.LogInformation($"Send ReduceFinishedEvent...");
        await _publisher.PublishAsync(new ReduceFinishedEvent(command.group, delta), _hostInfo.ReduceFinishedEventRoutingKey);
    }
}