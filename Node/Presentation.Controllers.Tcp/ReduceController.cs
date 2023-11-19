using Application.Commands;
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

    public async Task Consume(Reduce contract)
    {
        await _handler.Handle(contract, default);
    }
}
