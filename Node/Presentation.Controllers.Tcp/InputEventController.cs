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

    public Task Consume(InputCommand contract)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        _inputHandler.Handle(contract, cancellationTokenSource.Token);

        return Task.CompletedTask;
    }
}
