using Application.Commands.Seedwork;
using Domain.Publicity;

namespace Application.Commands.Map.Mapping;

public sealed record MapCommand(/*List<Space> spaces*/ int startIndex, int endIndex) : ICommand
{
    public string GetCommandName()
    {
        return "Map";
    }
}