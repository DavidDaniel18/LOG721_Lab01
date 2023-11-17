using Application.Commands.Seedwork;
using Domain.Grouping;

namespace Application.Commands.Reducer.Reduce;

public sealed record Reduce(Group group) : ICommand
{
    public string GetCommandName()
    {
        return "Reduce";
    }
}