using Application.Commands.Seedwork;
using Domain.Grouping;

namespace Application.Commands.Reducer.Event;

public sealed record ReduceFinishedEvent(Group group, double delta) : ICommand
{
    public string GetCommandName()
    {
        return "ReduceFinishedEvent";
    }
}