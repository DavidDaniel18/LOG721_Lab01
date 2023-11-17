using Application.Commands.Seedwork;
using Domain.Publicity;

public sealed record MapFinishedEvent(string name) : ICommand
{
    public string GetCommandName()
    {
        return "MapFinishedEvent";
    }
}