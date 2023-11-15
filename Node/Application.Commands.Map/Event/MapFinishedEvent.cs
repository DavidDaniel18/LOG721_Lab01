using Application.Commands.Seedwork;
using Domain.Grouping;
using Domain.Publicity;

public sealed record MapFinishedEvent(Space space, List<Group> groups) : ICommand
{
    public string GetCommandName()
    {
        return "MapFinishedEvent";
    }
}