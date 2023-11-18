using Application.Commands.Map.Event;
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

    public async Task Consume(MapFinishedEvent contract)
    {
        await _handler.Handle(contract, default);
    }
}
