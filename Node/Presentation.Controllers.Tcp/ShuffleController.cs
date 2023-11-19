using Application.Commands.Orchestrator.Shuffle;
using Application.Commands.Seedwork;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Tcp;

public class ShuffleController : IConsumer<Shuffle>
{
    private readonly ICommandHandler<Shuffle> _handler;

    public ShuffleController(ICommandHandler<Shuffle> handler)
    {
        _handler = handler;
    }

    public async Task Consume(Shuffle contract)
    {
        await _handler.Handle(contract, default);
    }
}
