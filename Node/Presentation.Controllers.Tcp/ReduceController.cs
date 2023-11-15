using Application.Commands;
using Application.Commands.Reducer;
using Application.Commands.Seedwork;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Tcp;

public class ReduceController : IConsumer<Reduce>
{
    private readonly ICommandHandler<Reduce> _handler;

    public ReduceController(ICommandHandler<Reduce> handler)
    {
        _handler = handler;
    }

    public Task Consume(Reduce contract)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        _handler.Handle(contract, cancellationTokenSource.Token);

        return Task.CompletedTask;
    }
}
