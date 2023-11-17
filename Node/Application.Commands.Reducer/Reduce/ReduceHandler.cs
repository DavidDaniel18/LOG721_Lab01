using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Reducer;

public sealed class ReduceHandler : ICommandHandler<Reduce>
{
    private readonly ILogger<ReduceHandler> _logger;

    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<ReduceFinishedEvent> _publisher;

    private ISingletonCache<Group> _groupsCache;

    public ReduceHandler(ILogger<ReduceHandler> logger, ISingletonCache<Group> groupsCache, IHostInfo hostInfo, IMessagePublisher<ReduceFinishedEvent> publisher)
    {
        _logger = logger;
        _groupsCache = groupsCache;
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public Task Handle(Reduce command, CancellationToken cancellation)
    {
        _logger.LogInformation($"Handle Reduce: {command.group.Id}...");
        _groupsCache.Value.TryAdd(command.group.Id, command.group);

        double avg = 0;
        
        foreach (var s in command.group.Spaces)
        {
            avg += s.GetNormalizedValue();
        }
        
        avg /= command.group.Spaces.Count();

        _logger.LogInformation($"New avg calculated: {avg}");

        if (_groupsCache.Value.TryGetValue(command.group.Id, out var group)) 
        {
            Group g = new Group(group.Id, avg, group.Spaces);
            _groupsCache.Value.TryUpdate(command.group.Id, g, group);

            _logger.LogInformation($"Send finished event...");
            Task.WaitAll(_publisher.PublishAsync(new ReduceFinishedEvent(g), _hostInfo.MapFinishedEventRoutingKey));
        }

        return Task.CompletedTask;
    }
}