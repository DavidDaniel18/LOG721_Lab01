using Application.Commands.Seedwork;

namespace Application.Commands.Orchestrator.Shuffle;

public sealed record Shuffle() : ICommand
{
    public string GetCommandName()
    {
        return "Shuffle";
    }
}