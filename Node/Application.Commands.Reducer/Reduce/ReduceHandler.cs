using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using SyncStore.Abstractions;
using System.Collections.Immutable;

namespace Application.Commands.Map.Event;

public sealed class ReduceHandler : ICommandHandler<Reduce>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<ReduceFinishedEvent> _publisher;

    private ISyncStore<string, Group> _groupsCache;
    private ISyncStore<string, Space> _spaceCache;

    public ReduceHandler(ISyncStore<string, Group> groupsCache, ISyncStore<string, Space> spaceCache, IHostInfo hostInfo, IMessagePublisher<ReduceFinishedEvent> publisher)
    {
        _spaceCache = spaceCache; 
        _groupsCache = groupsCache;
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public async Task Handle(Reduce command, CancellationToken cancellation)
    {
        var spaces = await _spaceCache.Query(q => q.Where(s => true));
        var groups = await _groupsCache.Query(q => q.Where(g => true));

        Dictionary<string, List<double>> barycentersByGroup = new Dictionary<string, List<double>>();

        double avg = 0;
        var spacesForGroup = spaces.Where(s => string.Equals(s.GroupId, command.group.Id));
        
        foreach (var s in spacesForGroup)
        {
            avg += s.GetNormalizedValue();
        }
        
        avg /= spacesForGroup.Count();

        await _groupsCache.AddOrUpdate(command.group.Id, new Group(command.group.Id, avg, ImmutableList<Space>.Empty));

        await _groupsCache.SaveChangesAsync();

        await _publisher.PublishAsync(new ReduceFinishedEvent(command.group), _hostInfo.MapFinishedEventRoutingKey);
    }
}