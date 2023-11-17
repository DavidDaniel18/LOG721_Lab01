using Application.Commands.Seedwork;
using Domain.Publicity;

namespace Application.Commands.Map.Mapping;

public sealed record MapCommand(List<Space> spaces) : ICommand
{
    public string GetCommandName()
    {
        return "Map";
    }
}