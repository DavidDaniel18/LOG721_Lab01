using Application.Commands.Seedwork;
using Domain.Grouping;

public sealed record ResultsCommand(Group group) : ICommand
{
    public string GetCommandName()
    {
        return "Results";
    }
}