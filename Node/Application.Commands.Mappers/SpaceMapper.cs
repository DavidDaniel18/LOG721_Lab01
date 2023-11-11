using Application.Dtos;
using Application.Mappers.Interfaces;
using Domain.Factories;
using Domain.Publicity;

namespace Application.Mappers;

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