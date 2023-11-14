using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using System.Text.RegularExpressions;

namespace Application.Commands.Map.Event;

public sealed class ReduceHandler : ICommandHandler<Reduce>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<ReduceFinishedEvent> _publisher;

    // DI data cache
    // _spaceCache
    private IEnumerable<Group> _groupsCache = new List<Group>(); // todo: link to cache

    public ReduceHandler(IHostInfo hostInfo, IMessagePublisher<ReduceFinishedEvent> publisher)
    {
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public Task Handle(Reduce command, CancellationToken cancellation)
    {
        // todo: do calculation + cache update.

        _publisher.PublishAsync(new ReduceFinishedEvent(command.group), _hostInfo.MapFinishedEventRoutingKey);

        return Task.CompletedTask;
    }
}