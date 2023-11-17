using Application.Commands.Seedwork;
using Domain.Grouping;

public sealed record ReduceFinishedEvent(Group group, double delta) : ICommand
{
    public string GetCommandName()
    {
        return "ReduceFinishedEvent";
    }
}