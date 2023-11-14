using Application.Commands.Seedwork;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Tcp;

public class ReduceFinishedEventController : IConsumer<ReduceFinishedEvent>
{
    private readonly ICommandHandler<ReduceFinishedEvent> _handler;

    public ReduceFinishedEventController(ICommandHandler<ReduceFinishedEvent> handler)
    {
        _handler = handler;
    }

    public Task Consume(ReduceFinishedEvent contract)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        _handler.Handle(contract, cancellationTokenSource.Token);

        return Task.CompletedTask;
    }
}
