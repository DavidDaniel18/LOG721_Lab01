using Application.Commands.Orchestrator.Interfaces;
using Application.Commands.Reducer.Reduce;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;
using SyncStore.Abstractions;

namespace Application.Commands.Orchestrator.Shuffle;

public sealed class ShuffleHander : ICommandHandler<Shuffle>
{
    private IMessagePublisher<Reduce> _publisher;

    private IGroupAttributionService _groupAttributionService;

    private ISyncStore<string, Group> _groupSyncStore;

    internal ShuffleHander(ISyncStore<string, Group> groupSyncStore, IMessagePublisher<Reduce> publisher, IGroupAttributionService groupAttributionService)
    {
        _groupSyncStore = groupSyncStore;
        _groupAttributionService = groupAttributionService;
        _publisher = publisher;
    }

    public async Task Handle(Shuffle command, CancellationToken cancellation)
    {
        var groups = await _groupSyncStore.Query(g => g);

        groups.ForEach(group => {
            Task.Run(() => _publisher.PublishAsync(new Reduce(group), _groupAttributionService.GetAttributedKeyFromGroup(group)), cancellation);
        });
    }
}