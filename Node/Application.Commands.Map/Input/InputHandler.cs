using Application.Commands.Algorithm;
using Application.Commands.Interfaces;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;

namespace Application.Commands.Map.Input;

internal sealed class InputHandler : ICommandHandler<Input>
{
    private readonly ICsvHandler _csvHandler;

    private readonly IMessagePublisher<Space> _publisher;

    // DI data cache
    // _spaceCache
    private IEnumerable<Space> _spacesCache = new List<Space>(); // todo: link to cache

    // DI group cache
    // _groupCache
    private IEnumerable<Group> _groupsCache = new List<Group>(); // todo: link to cache

    private readonly RoundRobinAlgorithm _algorithm;

    internal InputHandler(IHostInfo hostInfo, ICsvHandler csvHandler, IMessagePublisher<Space> publisher)
    {
        _csvHandler = csvHandler;
        _publisher = publisher;
        _algorithm = new RoundRobinAlgorithm(hostInfo.MapRoutingKeys.Split(',').ToList());
    }

    public Task Handle(Input command, CancellationToken cancellation)
    {
        if (!_spacesCache.Any() || !_groupsCache.Any()) // If not empty we continue (next iteration). Not super clean but could work.
        {
            Task.WaitAll(new Task[] {
                Task.Run(() => SyncDataToCache(), cancellation),
                Task.Run(() => SyncGroupToCache(), cancellation) // why not sync group also... (not needed)
            }, cancellation);
        }

        foreach (var space in _spacesCache)
            _publisher.PublishAsync(space, _algorithm.GetNextElement());

        return Task.CompletedTask;

        void SyncDataToCache()
        {
            _csvHandler.ReadDatas().ToList().AsParallel().ForAll(data => /* _spaceCache.AddOrUpdate(data); */ Console.WriteLine($"{data}"));
        }

        void SyncGroupToCache()
        {
            _csvHandler.ReadGroups().ToList().AsParallel().ForAll(group => /* _groupCache.AddOrUpdate(group); */ Console.WriteLine($"{group}"));
        }
    }
}