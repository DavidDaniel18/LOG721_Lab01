using Application.Commands.Seedwork;
using Domain.Grouping;

public sealed record ReduceFinishedEvent(Group space) : ICommand
{
    public string GetCommandName()
    {
        return "ReduceFinishedEvent";
    }
}