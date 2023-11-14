using Application.Commands.Seedwork;
using Domain.Publicity;

public sealed record MapFinishedEvent(Space space) : ICommand
{
    public string GetCommandName()
    {
        return "MapFinishedEvent";
    }
}