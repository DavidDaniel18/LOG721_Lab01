using Application.Commands.Seedwork;
using Domain.Grouping;

public sealed record ResultFinishedEvent(Group space) : ICommand
{
    public string GetCommandName()
    {
        return "ResultFinishedEvent";
    }
}