using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using Microsoft.Extensions.Logging;
using SyncStore.Abstractions;
using System.Collections.Immutable;

namespace Application.Commands.Reducer.Reduce;

public sealed class ReduceHandler : ICommandHandler<Reduce>
{
    private readonly ILogger<ReduceHandler> _logger;

    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<ReduceFinishedEvent> _publisher;

    private ISyncStore<string, Group> _groupsCache;

    public ReduceHandler(ILogger<ReduceHandler> logger, ISyncStore<string, Group> groupsCache, IHostInfo hostInfo, IMessagePublisher<ReduceFinishedEvent> publisher)
    {
        _logger = logger;
        _groupsCache = groupsCache;
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public async Task Handle(Reduce command, CancellationToken cancellation)
    {
        _logger.LogInformation($"Handler: {command.GetCommandName()}: Received");
        var spacesForGroup = command.group.Spaces;

        _logger.LogInformation($"Calculate avg for groupId: {command.group.Id}");
        double avg = 0;
        spacesForGroup.ForEach(s => avg += s.GetNormalizedValue());
        avg /= spacesForGroup.Count();

        _logger.LogInformation($"Save avg and emptied the spaces list of the group...");
        await _groupsCache.AddOrUpdate(command.group.Id, new Group(command.group.Id, avg, ImmutableList<Space>.Empty));
        await _groupsCache.SaveChangesAsync();

        _logger.LogInformation($"Send ReduceFinishedEvent...");
        await _publisher.PublishAsync(new ReduceFinishedEvent(command.group), _hostInfo.MapFinishedEventRoutingKey);
    }
}