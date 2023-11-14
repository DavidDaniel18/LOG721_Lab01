using Application.Commands.Seedwork;
using Domain.Grouping;

public sealed record Reduce(Group group) : ICommand
{
    public string GetCommandName()
    {
        return "Reduce";
    }
}