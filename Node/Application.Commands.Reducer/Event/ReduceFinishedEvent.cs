using Application.Commands.Seedwork;
using Domain.Grouping;

public sealed record ReduceFinishedEvent(Group group) : ICommand
{
    public string GetCommandName()
    {
        return "ReduceFinishedEvent";
    }
}