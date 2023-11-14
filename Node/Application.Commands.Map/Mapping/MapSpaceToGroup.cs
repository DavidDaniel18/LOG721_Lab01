using Application.Commands.Seedwork;
using Application.Dtos;
using Domain.Publicity;

namespace Application.Commands.Map.Mapping;

public sealed record Map(Space space) : ICommand
{
    public string GetCommandName()
    {
        return "Map";
    }
}