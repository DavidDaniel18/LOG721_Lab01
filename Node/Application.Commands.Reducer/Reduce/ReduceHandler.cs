using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using SyncStore.Abstractions;
using System.Collections.Immutable;

namespace Application.Commands.Reducer.Reduce;

public sealed class ReduceHandler : ICommandHandler<Reduce>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<ReduceFinishedEvent> _publisher;

    private ISyncStore<string, Group> _groupsCache;

    public ReduceHandler(ISyncStore<string, Group> groupsCache, IHostInfo hostInfo, IMessagePublisher<ReduceFinishedEvent> publisher)
    {
        _groupsCache = groupsCache;
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public async Task Handle(Reduce command, CancellationToken cancellation)
    {
        var spacesForGroup = command.group.Spaces;

        double avg = 0;
        spacesForGroup.ForEach(s => avg += s.GetNormalizedValue());
        avg /= spacesForGroup.Count();

        await _groupsCache.AddOrUpdate(command.group.Id, new Group(command.group.Id, avg, ImmutableList<Space>.Empty));
        await _groupsCache.SaveChangesAsync();

        await _publisher.PublishAsync(new ReduceFinishedEvent(command.group), _hostInfo.MapFinishedEventRoutingKey);
    }
}