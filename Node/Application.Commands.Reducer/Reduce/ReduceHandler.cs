using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;

namespace Application.Commands.Reducer;

public sealed class ReduceHandler : ICommandHandler<Reduce>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<ReduceFinishedEvent> _publisher;

    private ISingletonCache<Group> _groupsCache;

    public ReduceHandler(ISingletonCache<Group> groupsCache, IHostInfo hostInfo, IMessagePublisher<ReduceFinishedEvent> publisher)
    {
        _groupsCache = groupsCache;
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public Task Handle(Reduce command, CancellationToken cancellation)
    {
        _groupsCache.Value.TryAdd(command.group.Id, command.group);

        double avg = 0;
        
        foreach (var s in command.group.Spaces)
        {
            avg += s.GetNormalizedValue();
        }
        
        avg /= command.group.Spaces.Count();

        if (_groupsCache.Value.TryGetValue(command.group.Id, out var group)) 
        {
            Group g = new Group(group.Id, avg, group.Spaces);
            _groupsCache.Value.TryUpdate(command.group.Id, g, group);

            _publisher.PublishAsync(new ReduceFinishedEvent(g), _hostInfo.MapFinishedEventRoutingKey);
        }

        return Task.CompletedTask;
    }
}