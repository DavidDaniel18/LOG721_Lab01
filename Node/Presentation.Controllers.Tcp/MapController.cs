using Application.Commands.Map.Mapping;
using Application.Commands.Seedwork;
using Domain.Publicity;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Tcp;

public class MapController : IConsumer<Map>
{
    private readonly ICommandHandler<Map> _handler;
    
    public MapController(ICommandHandler<Map> handler)
    {
        _handler = handler;
    }

    public Task Consume(Map contract)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        _handler.Handle(contract, cancellationTokenSource.Token);

        return Task.CompletedTask;
    }
}
