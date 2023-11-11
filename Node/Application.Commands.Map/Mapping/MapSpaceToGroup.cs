using Application.Commands.Seedwork;
using Application.Dtos;

namespace Application.Commands.Map.Mapping;

public sealed record MapSpaceToGroup(List<SpaceDto> SpaceDtos, List<GroupDto> GroupDtos) : ICommand
{
    public string GetCommandName()
    {
        return "Map";
    }
}