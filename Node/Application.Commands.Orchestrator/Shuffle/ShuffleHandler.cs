using Application.Commands.Orchestrator.Interfaces;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;

namespace Application.Commands.Orchestrator.Shuffle;

public sealed class ShuffleHander : ICommandHandler<Shuffle>
{
    private IMessagePublisher<Reduce> _publisher;

    private IGroupAttributionService _groupAttributionService;

    private ISingletonCache<Group> _groupCache;

    public ShuffleHander(ISingletonCache<Space> spaceCache, ISingletonCache<Group> groupCache, IMessagePublisher<Reduce> publisher, IGroupAttributionService groupAttributionService)
    {
        _groupCache = groupCache;
        _groupAttributionService = groupAttributionService;
        _publisher = publisher;
    }

    public Task Handle(Shuffle command, CancellationToken cancellation)
    {
        _groupCache.Value.Values.ToList().ForEach(group => _publisher.PublishAsync(new Reduce(group), _groupAttributionService.GetAttributedKeyFromGroup(group)));

        return Task.CompletedTask;
    }
}