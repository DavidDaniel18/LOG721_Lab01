using Application.Commands.Mappers.Interfaces;
using Application.Commands.Seedwork;
using Application.Dtos;
using Domain.Publicity;
using Domain.Services;

namespace Application.Commands.Map.Mapping;

internal sealed class MapSpaceToGroupHandler : ICommandHandler<MapSpaceToGroup>
{
    private readonly IMappingTo<SpaceDto, Space> _spaceMapper;

    internal MapSpaceToGroupHandler(IMappingTo<SpaceDto, Space> spaceMapper)
    {
        _spaceMapper = spaceMapper;
    }

    public Task Handle(MapSpaceToGroup command, CancellationToken cancellation)
    {
        foreach (var space in command.SpaceDtos.Select(_spaceMapper.MapFrom))
        {
            GroupServices.GetClosestBarycentre(space, );
        }

        throw new NotImplementedException();
    }
}