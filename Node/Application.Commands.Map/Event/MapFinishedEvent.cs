using Application.Commands.Seedwork;

namespace Application.Commands.Map.Event;

public sealed record MapFinishedEvent(string Name) : ICommand
{
    public string GetCommandName()
    {
        return "MapFinishedEvent";
    }
}