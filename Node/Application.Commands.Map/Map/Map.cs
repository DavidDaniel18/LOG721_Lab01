using Application.Commands.Seedwork;
using Application.Dtos;
using Domain.Grouping;
using Domain.Publicity;

namespace Application.Commands.Map.Mapping;

public sealed record MapCommand(Space space, List<Group> groups) : ICommand
{
    public string GetCommandName()
    {
        return "Map";
    }
}