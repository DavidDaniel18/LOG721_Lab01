using Domain.Services.Common;

namespace Application.Services.InfrastructureInterfaces;

public interface ITcpBridge : IComHandler
{
    Task RunAsync(Stream inputStream, Stream outputStream, CancellationTokenSource cancellationTokenSource);
}