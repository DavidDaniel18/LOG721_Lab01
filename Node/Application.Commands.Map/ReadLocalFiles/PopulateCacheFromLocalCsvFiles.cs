using Application.Commands.Seedwork;

namespace Application.Commands.Map.ReadLocalFiles;

internal sealed class PopulateCacheFromLocalCsvFiles : ICommand
{
    public string GetCommandName()
    {
        return "PopulateCacheFromLocalCsvFiles";
    }
}