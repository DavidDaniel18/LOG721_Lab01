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

    public Task Consume(Shuffle contract)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        _handler.Handle(contract, cancellationTokenSource.Token);

        return Task.CompletedTask;
    }
}
