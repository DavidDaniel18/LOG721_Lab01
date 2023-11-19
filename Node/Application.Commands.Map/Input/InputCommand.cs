using Application.Commands.Seedwork;

namespace Application.Commands.Map.Input;

public sealed class InputCommand : ICommand
{
    public InputCommand(string dataCsvFileName, string groupCsvFileName)
    {
        this.dataCsvFileName = dataCsvFileName;
        this.groupCsvFileName = groupCsvFileName;
    }

    public string GetCommandName()
    {
        return "Input";
    }

    public string dataCsvFileName { get; set; }
    public string groupCsvFileName { get; set; }
}