using Domain.Services.Common;

namespace Application.Services.InfrastructureInterfaces;

public interface ITcpBridge : IComHandler
{
    bool IsCompleted();
    
    bool IsNotStarted();

    void RunAsync(Stream inputStream, Stream outputStream, CancellationTokenSource cancellationTokenSource);
}