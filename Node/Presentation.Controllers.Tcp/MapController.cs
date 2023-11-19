using Application.Commands.Map.Map;
using Application.Commands.Seedwork;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Tcp;

public class MapController : IConsumer<MapCommand>
{
    private readonly ICommandHandler<MapCommand> _handler;
    
    public MapController(ICommandHandler<MapCommand> handler)
    {
        _handler = handler;
    }

    public async Task Consume(MapCommand contract)
    {
        await _handler.Handle(contract, default);
    }
}
