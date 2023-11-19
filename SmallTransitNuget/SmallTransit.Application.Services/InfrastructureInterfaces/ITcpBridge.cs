using SmallTransit.Domain.Services.Common;

namespace SmallTransit.Application.Services.InfrastructureInterfaces;

public interface ITcpBridge : IComHandler
{
    bool IsCompleted();
    
    bool IsNotStarted();

    void RunAsync(Stream inputStream, Stream outputStream, CancellationTokenSource cancellationTokenSource);
}