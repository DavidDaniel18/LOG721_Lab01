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

        var spaces = await _spacesCache.Query(s => s.Where(space => space.GroupId!.Equals(command.group.Id)));

        double barycentre = command.group.Barycentre;

        _logger.LogInformation($"Calculate avg for groupId: {command.group.Id}");
        double avg = 0;
        spaces.ForEach(s => avg += s.GetNormalizedValue());
        avg /= spaces.Count();

        _logger.LogInformation($"Save avg {avg} and emptied the spaces list of the group...");
        await _groupsCache.AddOrUpdate(command.group.Id, new Group(command.group.Id, avg, ImmutableList<Space>.Empty));
        await _groupsCache.SaveChangesAsync();

        _logger.LogInformation($"Send ReduceFinishedEvent...");
        await _publisher.PublishAsync(new ReduceFinishedEvent(command.group, Math.Abs(avg - barycentre)), _hostInfo.ReduceFinishedEventRoutingKey);
    }
}