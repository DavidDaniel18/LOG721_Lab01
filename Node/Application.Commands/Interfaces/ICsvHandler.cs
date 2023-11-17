using Application.Dtos;

namespace Application.Commands.Interfaces;

public interface ICsvHandler
{
    public IAsyncEnumerable<DataDto> ReadDatasAsync();
    public IAsyncEnumerable<GroupDto> ReadGroupsAsync();
    public IEnumerable<DataDto> ReadDatas();
    public IEnumerable<GroupDto> ReadGroups();
}