using Application.Commands.Seedwork;

namespace Application.Commands.Map.Input;

public sealed record InputCommand(string dataCsvFileName, string groupCsvFileName) : ICommand
{
    public string GetCommandName()
    {
        return "Input";
    }
}