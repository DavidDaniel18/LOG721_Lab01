using Application.Commands.Seedwork;

namespace Application.Commands.Map.Map;

public sealed record MapCommand(List<string> SpaceIds) : ICommand
{
    public string GetCommandName()
    {
        return "Map";
    }
}