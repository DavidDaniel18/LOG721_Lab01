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

    private ISyncStore<string, Space> _spaceSyncStore;

    private ISyncStore<string, MapFinishedBool> _spaceFinishedSyncStore;

    public MapFinishedEventHandler(
        ISyncStore<string, Space> spaceSyncStore,
        ISyncStore<string, MapFinishedBool> spaceFinishedSyncStore,
        IHostInfo hostInfo, 
        IMessagePublisher<Shuffle> publisher)
    {
        _spaceFinishedSyncStore = spaceFinishedSyncStore;
        _spaceSyncStore = spaceSyncStore;  
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public async Task Handle(MapFinishedEvent command, CancellationToken cancellation)
    {
        await _spaceFinishedSyncStore.AddOrUpdate(command.space.Id, new MapFinishedBool { Value = true, Id = command.space.Id });

        await _spaceFinishedSyncStore.SaveChangesAsync();

        var spaces = await _spaceSyncStore.Query(query => query.Where(space => true));
        var result = await _spaceFinishedSyncStore.Query(query => query.Where(space => space.Value));

        bool isFinished = result.All(s => s.Value) && result.Count() == spaces.Count();

        if (isFinished)
        {
            //result.ForEach(r => _spaceFinishedSyncStore.AddOrUpdate(r.Id, new MapFinishedBool { Value = false, Id = r.Id }));
            //await _spaceFinishedSyncStore.SaveChangesAsync();

            await _publisher.PublishAsync(new Shuffle(), _hostInfo.MapShuffleRoutingKey);
        }
    }
}