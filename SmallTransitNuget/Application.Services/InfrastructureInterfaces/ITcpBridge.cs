using Domain.Services.Common;

namespace Application.Services.InfrastructureInterfaces;

public interface ITcpBridge : IComHandler
{
    void RunAsync(Stream inputStream, Stream outputStream, CancellationTokenSource cancellationTokenSource);
}