using Application.Commands.Orchestrator.Interfaces;
using Application.Commands.Reducer.Reduce;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using Microsoft.Extensions.Logging;
using SyncStore.Abstractions;

namespace Application.Commands.Orchestrator.Shuffle;

public sealed class ShuffleHandler : ICommandHandler<Shuffle>
{
    private readonly ILogger<ShuffleHandler> _logger;

    private IMessagePublisher<Reduce> _publisher;

    private IGroupAttributionService _groupAttributionService;

    private ISyncStore<string, Group> _groupSyncStore;

    public ShuffleHandler(ILogger<ShuffleHandler> logger, ISyncStore<string, Group> groupSyncStore, IMessagePublisher<Reduce> publisher, IGroupAttributionService groupAttributionService)
    {
        _logger = logger;
        _groupSyncStore = groupSyncStore;
        _groupAttributionService = groupAttributionService;
        _publisher = publisher;
    }

    public async Task Handle(Shuffle command, CancellationToken cancellation)
    {
        _logger.LogInformation($"Handler: {command.GetCommandName()}: Received");
        var groups = await _groupSyncStore.Query(g => g);

        _logger.LogInformation("Send groups to appropriate reduce worker...");
        groups.ForEach(group => {
            Task.Run(() => _publisher.PublishAsync(new Reduce(group), _groupAttributionService.GetAttributedKeyFromGroup(group)), cancellation);
        });
    }
}