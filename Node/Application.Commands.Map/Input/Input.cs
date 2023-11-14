using Application.Commands.Seedwork;

namespace Application.Commands.Map.Input;

public sealed record Input(string dataCsvFileName, string groupCsvFileName) : ICommand
{
    public string GetCommandName()
    {
        return "Input";
    }
}