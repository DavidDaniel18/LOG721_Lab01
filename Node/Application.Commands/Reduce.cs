using Application.Commands.Seedwork;
using Domain.Grouping;

namespace Application.Commands;

public sealed record Reduce(Group group) : ICommand
{
    public string GetCommandName()
    {
        return "Reduce";
    }
}