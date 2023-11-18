using Application.Commands.Seedwork;

namespace Application.Commands.Map.Map;

public sealed record MapCommand(/*List<Space> spaces*/ int StartIndex, int EndIndex) : ICommand
{
    public string GetCommandName()
    {
        return "Map";
    }
}