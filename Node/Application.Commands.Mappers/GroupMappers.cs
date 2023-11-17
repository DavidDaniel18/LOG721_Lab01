using Application.Commands.Mappers.Interfaces;
using Application.Dtos;
using Domain.Factories;
using Domain.Grouping;

namespace Application.Commands.Mappers;

public sealed class GroupMapper : IMappingTo<GroupDto, Group>
{
    public Group MapFrom(GroupDto dto)
    {
        return GroupFactory.Create(dto.Id, dto.Value);
    }
}