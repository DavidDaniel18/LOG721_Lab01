using Application.Commands.Algorithm;
using Application.Commands.Interfaces;
using Application.Commands.Map.Mapping;
using Application.Commands.Mappers.Interfaces;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Grouping;
using Domain.Publicity;
using Microsoft.Extensions.Logging;
using SyncStore.Abstractions;

namespace Application.Commands.Map.Input;

public sealed class InputHandler : ICommandHandler<InputCommand>
{
    private readonly ILogger<InputHandler> _logger;

    private readonly ICsvHandler _csvHandler;

    private readonly IMessagePublisher<MapCommand> _publisher;

    private readonly IMappingTo<DataDto, Space> _dataMapper;

    private readonly IMappingTo<GroupDto, Group> _groupMapper;

    private ISyncStore<string, Space> _spacesCache;

    private ISyncStore<string, Group> _groupsCache;

    private IHostInfo _hostInfo;

    private readonly RoundRobinAlgorithm _algorithm;

    public InputHandler(
        ILogger<InputHandler> logger,
        ISyncStore<string, Space> spaceSyncStore, 
        ISyncStore<string, Group> groupSyncStore,
        IMappingTo<DataDto, Space> dataMapper,
        IMappingTo<GroupDto, Group> groupMapper,
        IHostInfo hostInfo, 
        ICsvHandler csvHandler, 
        IMessagePublisher<MapCommand> publisher)
    {
        _logger = logger;
        _hostInfo = hostInfo;
        _dataMapper = dataMapper;
        _groupMapper = groupMapper;
        _csvHandler = csvHandler;
        _publisher = publisher;
        _spacesCache = spaceSyncStore;
        _groupsCache = groupSyncStore;
        _algorithm = new RoundRobinAlgorithm(hostInfo.MapRoutingKeys.Split(',').ToList());
    }

    public async Task Handle(InputCommand command, CancellationToken cancellation)
    {
        _logger.LogInformation($"Handler: {command.GetCommandName()}: Received");

        var spaces = await _spacesCache.Query(s => s);
        var groups = await _groupsCache.Query(g => g);
        
        if (!spaces.Any() || !groups.Any())
        {
            _logger.LogInformation("Init of Caches: Groups and Spaces...");
            Task.WaitAll(new Task[] {
                Task.Run(() => SyncDataToCache(), cancellation),
                Task.Run(() => SyncGroupToCache(), cancellation)
            }, cancellation);
            _logger.LogInformation("Init of Caches: Terminated");
        }

        _logger.LogInformation("Map spaces to map workers...");
        await Task.Run(() => SendSpacesToMappers());

        async void SyncDataToCache()
        {
            await _spacesCache.AddOrUpdateRange(_csvHandler.ReadDatas().ToList().Select(dto =>
            {
                var space = _dataMapper.MapFrom(dto);
                return (space.Id, space);
            }));

            await _spacesCache.SaveChangesAsync();
        }

        async void SyncGroupToCache()
        {
            await _groupsCache.AddOrUpdateRange(_csvHandler.ReadGroups().ToList().Select(dto =>
            {
                var group = _groupMapper.MapFrom(dto);
                return (group.Id, group);
            }));

            await _groupsCache.SaveChangesAsync();
        }

        async void SendSpacesToMappers()
        {
            var spaces = await _spacesCache.Query(q => q);

            var mapTopics = _hostInfo.MapRoutingKeys.Split(',').ToList();

            int nbOfMapTopics = mapTopics.Count();
            int chunkSize = spaces.Count() / mapTopics.Count();

            var tasks = new Task[nbOfMapTopics];

            for (int i = 0; i < nbOfMapTopics; i++)
            {
                var startIndex = i * chunkSize;
                string mapTopic = _algorithm.GetNextElement();

                _logger.LogInformation($"Send spaces[{startIndex}, {startIndex + chunkSize - 1}] to {mapTopic}"); 
                
                tasks[i] = Task.Run(async () => 
                {
                    try
                    {
                        var elements = spaces.GetRange(startIndex, chunkSize);
                
                        _logger.LogInformation($"nb of spaces to send: {elements.Count()}");
                
                        _logger.LogInformation($"Sending...");
                        await _publisher.PublishAsync(new MapCommand(startIndex, startIndex + chunkSize - 1), mapTopic);
                        _logger.LogInformation($"Spaces sent...");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                });
            }

            Task.WaitAll(tasks);
        }
    }
}