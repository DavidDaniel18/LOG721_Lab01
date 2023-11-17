using Application.Commands.Orchestrator.Interfaces;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Orchestrator.Shuffle;

public sealed class ShuffleHander : ICommandHandler<Shuffle>
{
    private ILogger<ShuffleHander> _logger;

    private IMessagePublisher<Reduce> _publisher;

    private IGroupAttributionService _groupAttributionService;

    private ISingletonCache<Group> _groupCache;

    public ShuffleHander(ISingletonCache<Space> spaceCache, ILogger<ShuffleHander> logger, ISingletonCache<Group> groupCache, IMessagePublisher<Reduce> publisher, IGroupAttributionService groupAttributionService)
    {
        _logger = logger;
        _groupCache = groupCache;
        _groupAttributionService = groupAttributionService;
        _publisher = publisher;
    }

    public Task Handle(Shuffle command, CancellationToken cancellation)
    {
        _logger.LogInformation("Shuffle command captured...");
        _logger.LogInformation("Distribute reduce operations...");
        _groupCache.Value.Values.ToList().ForEach(group => {
            string topic = _groupAttributionService.GetAttributedKeyFromGroup(group);
            _logger.LogInformation($"Send reduce command to: {topic}");
            Task.WaitAll(_publisher.PublishAsync(new Reduce(group), _groupAttributionService.GetAttributedKeyFromGroup(group)));
        });

        return Task.CompletedTask;
    }
}