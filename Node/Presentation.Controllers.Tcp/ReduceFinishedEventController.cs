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

    public async Task Consume(ReduceFinishedEvent contract)
    {
        await _handler.Handle(contract, default);
    }
}
