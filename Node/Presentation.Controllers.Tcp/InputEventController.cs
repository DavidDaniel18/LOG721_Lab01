using Application.Commands.Map.Input;
using Application.Commands.Seedwork;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Tcp;

public class InputEventController : IConsumer<Input>
{
    private readonly ICommandHandler<Input> _inputHandler;

    public InputEventController(ICommandHandler<Input> inputHandler)
    {
        _inputHandler = inputHandler;
    }

    public Task Consume(Input contract)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        _inputHandler.Handle(contract, cancellationTokenSource.Token);

        return Task.CompletedTask;
    }
}
