using Application.Commands.Seedwork;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Tcp;

public class MapFinishedEventController : IConsumer<MapFinishedEvent>
{
    private readonly ICommandHandler<MapFinishedEvent> _handler;

    public MapFinishedEventController(ICommandHandler<MapFinishedEvent> handler)
    {
        _handler = handler;
    }

    public Task Consume(MapFinishedEvent contract)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        _handler.Handle(contract, cancellationTokenSource.Token);

        return Task.CompletedTask;
    }
}
