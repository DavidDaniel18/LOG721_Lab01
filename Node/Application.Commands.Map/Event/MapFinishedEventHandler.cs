using Application.Commands.Orchestrator.Shuffle;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Grouping;
using Domain.Publicity;
using Microsoft.Extensions.Logging;
using SyncStore.Abstractions;

namespace Application.Commands.Map.Event;

public sealed class MapFinishedEventHandler : ICommandHandler<MapFinishedEvent>
{
    private readonly ILogger<MapFinishedEventHandler> _logger;

    private readonly IHostInfo _hostInfo;

    private readonly IMessagePublisher<Shuffle> _publisher;

    private ISyncStore<string, MapFinishedBool> _spaceFinishedSyncStore;

    public MapFinishedEventHandler(
        ILogger<MapFinishedEventHandler> logger,
        ISyncStore<string, MapFinishedBool> spaceFinishedSyncStore,
        IHostInfo hostInfo, 
        IMessagePublisher<Shuffle> publisher)
    {
        _logger = logger;
        _spaceFinishedSyncStore = spaceFinishedSyncStore;
        _hostInfo = hostInfo;
        _publisher = publisher;
    }

    public async Task Handle(MapFinishedEvent command, CancellationToken cancellation)
    {
        _logger.LogInformation($"Handler: {command.GetCommandName()}: Received");

        _logger.LogInformation($"Save the {command.name} result");
        await _spaceFinishedSyncStore.AddOrUpdate(command.name, new MapFinishedBool { IsFinished = true, Id = command.name });
        await _spaceFinishedSyncStore.SaveChangesAsync();

        _logger.LogInformation($"Is the map step finished...");
        var mapJobsFinished = await _spaceFinishedSyncStore.Query(query => query.Where(space => space.IsFinished));

        bool isFinished = mapJobsFinished.Count() == _hostInfo.MapRoutingKeys.Split(',').Count();

        if (isFinished)
        {
            _logger.LogInformation($"Map step finished");

            _logger.LogInformation($"Reset finished cache");
            mapJobsFinished.ForEach(f => f.IsFinished = false);
            
            await _spaceFinishedSyncStore.AddOrUpdateRange(mapJobsFinished.Select(m => (m.Id, m)));
            await _spaceFinishedSyncStore.SaveChangesAsync();

            _logger.LogInformation($"Send Shuffle command...");
            await _publisher.PublishAsync(new Shuffle(), _hostInfo.MapShuffleRoutingKey);
        }
        else
        {
            _logger.LogInformation($"Map step not finished...");
        }
    }
}