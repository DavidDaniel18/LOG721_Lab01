using Application.Commands.Algorithm;
using Application.Commands.Interfaces;
using Application.Commands.Map.Mapping;
using Application.Commands.Mappers.Interfaces;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Grouping;
using Domain.Publicity;
using SyncStore.Abstractions;

namespace Application.Commands.Map.Input;

public sealed class InputHandler : ICommandHandler<InputCommand>
{
    private readonly ICsvHandler _csvHandler;

    private readonly IMessagePublisher<MapCommand> _publisher;

    private readonly IMappingTo<DataDto, Space> _dataMapper;

    private readonly IMappingTo<GroupDto, Group> _groupMapper;

    private ISyncStore<string, Space> _spacesCache;

    private ISyncStore<string, Group> _groupsCache;

    private readonly RoundRobinAlgorithm _algorithm;

    public InputHandler(
        ISyncStore<string, Space> spaceSyncStore, 
        ISyncStore<string, Group> groupSyncStore,
        IMappingTo<DataDto, Space> dataMapper,
        IMappingTo<GroupDto, Group> groupMapper,
        IHostInfo hostInfo, 
        ICsvHandler csvHandler, 
        IMessagePublisher<MapCommand> publisher)
    {
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
        var spaces = await _spacesCache.Query(spaces => spaces.Where(space => true));
        var groups = await _groupsCache.Query(groups => groups.Where(group => true));
        
        if (!spaces.Any() || !groups.Any()) // If not empty we continue (next iteration). Not super clean but could work.
        {
            Task.WaitAll(new Task[] {
                Task.Run(() => SyncDataToCache(), cancellation),
                Task.Run(() => SyncGroupToCache(), cancellation) // why not sync group also... (not needed)
            }, cancellation);
        }

        foreach (var space in spaces)
            _ = _publisher.PublishAsync(new MapCommand(space), _algorithm.GetNextElement());

        void SyncDataToCache()
        {
            _csvHandler.ReadDatas().ToList().AsParallel().ForAll(async dto => 
            {
                var space = _dataMapper.MapFrom(dto);
                await _spacesCache.AddOrUpdate(space.Id, space);
                
            });
            _spacesCache.SaveChangesAsync().ConfigureAwait(true);
        }

        void SyncGroupToCache()
        {
            _csvHandler.ReadGroups().ToList().AsParallel().ForAll(async dto =>
            {
                var group = _groupMapper.MapFrom(dto);
                await _groupsCache.AddOrUpdate(group.Id, group);
            });
            _groupsCache.SaveChangesAsync().ConfigureAwait(true);
        }
    }
}