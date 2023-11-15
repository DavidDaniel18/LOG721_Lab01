using Application.Commands.Algorithm;
using Application.Commands.Interfaces;
using Application.Commands.Map.Mapping;
using Application.Commands.Mappers.Interfaces;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Common;
using Domain.Grouping;
using Domain.Publicity;

namespace Application.Commands.Map.Input;

public sealed class InputHandler : ICommandHandler<InputCommand>
{
    private readonly ICsvHandler _csvHandler;

    private readonly IMessagePublisher<MapCommand> _publisher;

    private readonly IMappingTo<DataDto, Space> _dataMapper;

    private readonly IMappingTo<GroupDto, Group> _groupMapper;

    private ISingletonCache<Space> _spaceCache;

    private ISingletonCache<Group> _groupCache;

    private ISingletonCache<MapFinishedBool> _spaceFinishedCache;

    private readonly RoundRobinAlgorithm _algorithm;

    public InputHandler(
        ISingletonCache<Space> spaceCache,
        ISingletonCache<Group> groupCache,
        ISingletonCache<MapFinishedBool> spaceFinishedCache,
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
        _spaceFinishedCache = spaceFinishedCache;
        _spaceCache = spaceCache;
        _groupCache = groupCache;
        _algorithm = new RoundRobinAlgorithm(hostInfo.MapRoutingKeys.Split(',').ToList());
    }

    public Task Handle(InputCommand command, CancellationToken cancellation)
    {
        _spaceFinishedCache.Value.Clear();

        var spaces = _spaceCache.Value.Values;
        var groups = _groupCache.Value.Values;
        
        if (!spaces.Any() || !groups.Any()) // If not empty we continue (next iteration). Not super clean but could work.
        {
            Task.WaitAll(new Task[] {
                Task.Run(() => SyncDataToCache(), cancellation),
                Task.Run(() => SyncGroupToCache(), cancellation) // why not sync group also... (not needed)
            }, cancellation);
        }

        var groupsList = groups.ToList();

        foreach (var space in spaces)
            _ = _publisher.PublishAsync(new MapCommand(space, groupsList), _algorithm.GetNextElement());

        return Task.CompletedTask;

        void SyncDataToCache()
        {
            _csvHandler.ReadDatas().ToList().AsParallel().ForAll(dto => 
            {
                var space = _dataMapper.MapFrom(dto);
                _spaceCache.Value.TryAdd(space.Id, space);
            });
            //_spacesCache.SaveChangesAsync().ConfigureAwait(true);
        }

        void SyncGroupToCache()
        {
            _csvHandler.ReadGroups().ToList().AsParallel().ForAll(dto =>
            {
                var group = _groupMapper.MapFrom(dto);
                _groupCache.Value.TryAdd(group.Id, group);
            });
            //_groupsCache.SaveChangesAsync().ConfigureAwait(true);
        }
    }
}