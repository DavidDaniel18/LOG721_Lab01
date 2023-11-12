using Application.Commands.Orchestrator.Interfaces;
using Application.Commands.Seedwork;
using Application.Common.Interfaces;
using Domain.Publicity;

namespace Application.Commands.Orchestrator.Shuffle;

internal sealed class ShuffleHander : ICommandHandler<Shuffle>
{
    private IMessagePublisher<Space> _publisher;

    private IGroupAttributionService _groupAttributionService;

    private IEnumerable<Space> _spaces = new List<Space>(); // todo: use cache.

    internal ShuffleHander(IMessagePublisher<Space> publisher, IGroupAttributionService groupAttributionService)
    {
        _groupAttributionService = groupAttributionService;
        _publisher = publisher;
    }

    public Task Handle(Shuffle command, CancellationToken cancellation)
    {
        _spaces.Where(s => s.GroupId != null).ToList().ForEach(space => {
            Task.Run(() => _publisher.PublishAsync(space, _groupAttributionService.GetAttributedKeyFromSpace(space)), cancellation);
        });

        return Task.CompletedTask;
    }
}