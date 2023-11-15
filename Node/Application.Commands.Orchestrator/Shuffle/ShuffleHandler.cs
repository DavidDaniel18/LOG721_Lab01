using Application.Commands.Orchestrator.Interfaces;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Publicity;
using SyncStore.Abstractions;

namespace Application.Commands.Orchestrator.Shuffle;

public sealed class ShuffleHander : ICommandHandler<Shuffle>
{
    private IMessagePublisher<Space> _publisher;

    private IGroupAttributionService _groupAttributionService;

    private ISyncStore<string, Space> _spaceSyncStore;

    internal ShuffleHander(ISyncStore<string, Space> spaceSyncStore, IMessagePublisher<Space> publisher, IGroupAttributionService groupAttributionService)
    {
        _spaceSyncStore = spaceSyncStore;
        _groupAttributionService = groupAttributionService;
        _publisher = publisher;
    }

    public async Task Handle(Shuffle command, CancellationToken cancellation)
    {
        var spaces = await _spaceSyncStore.Query(query => query.Where(s => s.GroupId != null));

        spaces.ForEach(space => {
            Task.Run(() => _publisher.PublishAsync(space, _groupAttributionService.GetAttributedKeyFromSpace(space)), cancellation);
        });
    }
}