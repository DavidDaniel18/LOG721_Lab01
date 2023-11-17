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
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.Commands.Map.Input;

public sealed class InputHandler : ICommandHandler<InputCommand>
{
    private readonly ICsvHandler _csvHandler;

    private readonly IMessagePublisher<MapCommand> _publisher;

    private ISingletonCache<Space> _spaceCache;

    private ISingletonCache<Group> _groupCache;

    private ISingletonCache<MapFinishedBool> _spaceFinishedCache;

    private ILogger<InputHandler> _logger;

    private readonly RoundRobinAlgorithm _algorithm;

    public InputHandler(
        ILogger<InputHandler> logger,
        ISingletonCache<Space> spaceCache,
        ISingletonCache<Group> groupCache,
        ISingletonCache<MapFinishedBool> spaceFinishedCache,
        IHostInfo hostInfo, 
        ICsvHandler csvHandler, 
        IMessagePublisher<MapCommand> publisher)
    {
        _logger = logger;
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
            _logger.LogInformation("Initiate cache...");
            Task.WaitAll(new Task[] {
                Task.Run(() => SyncDataToCache(), cancellation),
                Task.Run(() => SyncGroupToCache(), cancellation) // why not sync group also... (not needed)
            }, cancellation);
            _logger.LogInformation("Cache initiated...");
        }

        _logger.LogInformation($"Group cache count: {_groupCache.Value.Values.Count()}");
        _logger.LogInformation($"Space cache count: {_spaceCache.Value.Values.Count()}");

        var groupsList = _groupCache.Value.Values.ToList();
        foreach (var space in _spaceCache.Value.Values)
        {
            var topic = _algorithm.GetNextElement();
            _logger.LogInformation($"Send map command: {space.Id}, topic: {topic}");
            //try
            //{
                //Task.WaitAll(_publisher.PublishAsync(new MapCommand(space, groupsList), topic));
                Task.WaitAll(_publisher.PublishAsync(new MapCommand(space, new List<Group>()), topic));
            //}
            //catch (Exception e) 
            //{
            //    _logger.LogError(e.Message);
            //} 
        }

        _logger.LogInformation($"MapCommand Completed");
        return Task.CompletedTask;

        void SyncDataToCache()
        {
            _csvHandler.ReadDatas().ToList().ForEach(dto => 
            {
                var space = new Space(Guid.NewGuid().ToString(), dto.Width, dto.Price);
                _spaceCache.Value.TryAdd(space.Id, space);
            });
        }

        void SyncGroupToCache()
        {
            _csvHandler.ReadGroups().ToList().ForEach(dto =>
            {
                var group = new Group(dto.Id, dto.Value, new List<Space>());
                _groupCache.Value.TryAdd(group.Id, group);
            });
        }
    }
}