using Application.Commands.Map.Input;
using Application.Commands.Seedwork;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Tcp;

public class InputEventController : IConsumer<InputCommand>
{
    private readonly ICommandHandler<InputCommand> _inputHandler;

    public InputEventController(ICommandHandler<InputCommand> inputHandler)
    {
        _inputHandler = inputHandler;
    }

    public async Task Consume(InputCommand contract)
    {
        await _inputHandler.Handle(contract, default);
    }
}
