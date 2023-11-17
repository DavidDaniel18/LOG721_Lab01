using Application.Commands.Seedwork;

namespace Application.Commands.Map.ReadLocalFiles;

internal sealed class PopulateCacheFromLocalCsvFilesHandler : ICommandHandler<PopulateCacheFromLocalCsvFiles>
{
    internal PopulateCacheFromLocalCsvFilesHandler()
    {
        
    }

    public Task Handle(PopulateCacheFromLocalCsvFiles command, CancellationToken cancellation)
    {
        return Task.CompletedTask;
    }
}