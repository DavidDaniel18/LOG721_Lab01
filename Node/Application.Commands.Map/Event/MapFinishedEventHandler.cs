using Application.Commands.Orchestrator.Shuffle;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Grouping;
using Domain.Publicity;
using SyncStore.Abstractions;

namespace Application.Commands.Map.Event;

public sealed class MapFinishedEventHandler : ICommandHandler<MapFinishedEvent>
{
    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<Shuffle> _publisher;

    private ISyncStore<string, MapFinishedBool> _spaceFinishedSyncStore;

    public MapFinishedEventHandler(
        ISyncStore<string, Space> spaceSyncStore,
        ISyncStore<string, MapFinishedBool> spaceFinishedSyncStore,
        IHostInfo hostInfo, 
        IMessagePublisher<Shuffle> publisher)
    {
        _spaceFinishedSyncStore = spaceFinishedSyncStore;
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public async Task Handle(MapFinishedEvent command, CancellationToken cancellation)
    {
        await _spaceFinishedSyncStore.AddOrUpdate(command.name, new MapFinishedBool { IsFinished = true, Id = command.name });
        await _spaceFinishedSyncStore.SaveChangesAsync();

        var mapJobsFinished = await _spaceFinishedSyncStore.Query(query => query.Where(space => space.IsFinished));

        bool isFinished = mapJobsFinished.Count() == _hostInfo.MapRoutingKeys.Split(',').Count();

        if (isFinished)
        {
            mapJobsFinished.ForEach(f => f.IsFinished = false);
            
            await _spaceFinishedSyncStore.AddOrUpdateRange(mapJobsFinished.Select(m => (m.Id, m)));
            await _spaceFinishedSyncStore.SaveChangesAsync();

            await _publisher.PublishAsync(new Shuffle(), _hostInfo.MapShuffleRoutingKey);
        }
    }
}