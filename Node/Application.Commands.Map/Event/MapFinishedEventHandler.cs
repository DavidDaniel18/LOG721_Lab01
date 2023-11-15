using Application.Commands.Algorithm;
using Application.Commands.Orchestrator.Shuffle;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Publicity;

namespace Application.Commands.Map.Event;

public sealed class MapFinishedEventHandler : ICommandHandler<MapFinishedEvent>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<Shuffle> _publisher;

    // DI data cache
    // _spaceCache
    private IEnumerable<Space> _spacesCache = new List<Space>(); // todo: link to cache

    private Dictionary<string, bool> _spaceFinished = new Dictionary<string, bool>(); 

    public MapFinishedEventHandler(IHostInfo hostInfo, IMessagePublisher<Shuffle> publisher)
    {
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public Task Handle(MapFinishedEvent command, CancellationToken cancellation)
    {
        _spaceFinished.Add(command.space.Id, true);

        // todo: when all is true and size == cache.size send event finished
        bool isFinished = false;
        if (isFinished)
        {
            _publisher.PublishAsync(new Shuffle(), _hostInfo.MapShuffleRoutingKey);
        }

        return Task.CompletedTask;
    }
}