using Application.Commands.Mappers.Interfaces;
using Application.Dtos;
using Domain.Factories;
using Domain.Publicity;

namespace Application.Commands.Mappers;

public sealed class SpaceMapper : IMappingTo<SpaceDto, Space>, IMappingTo<DataDto, Space>
{
    public Space MapFrom(SpaceDto dto)
    {
        return PublicitySpaceFactory.Create(dto.Id, dto.Width, dto.Price);
    }

    public Space MapFrom(DataDto dto)
    {
        return PublicitySpaceFactory.Create(dto.Width, dto.Price);
    }
}