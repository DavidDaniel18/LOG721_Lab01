using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Application.Services.InfrastructureInterfaces;
using SmallTransit.Application.Services.Orchestrator.Sending;
using SmallTransit.Domain.Services.Sending.Send;

namespace SmallTransit.Application.UseCases;

public sealed class SendClient<TContract, TResult> : IDisposable
{
    private readonly ITcpBridge _tcpBridge;
    private readonly INetworkStreamCache _networkStreamCache;
    private readonly ILogger<PublishClient<TContract>> _logger;
    private readonly SendOrchestrator<TContract, TResult> _publishingSendingOrchestrator;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public SendClient(ITcpBridge tcpBridge, INetworkStreamCache networkStreamCache, ILogger<PublishClient<TContract>> logger)
    {
        _tcpBridge = tcpBridge;
        _networkStreamCache = networkStreamCache;
        _logger = logger;

        _publishingSendingOrchestrator = new SendOrchestrator<TContract, TResult>(new SendingContext(), tcpBridge);
    }

    internal async Task<Result<TResult>> Send(SendWrapper<TContract> sendWrapper, string targetId)
    {
        if (_tcpBridge.IsCompleted()) return Result.Failure<TResult>("Bridging task is completed");

        if (_tcpBridge.IsNotStarted())
        {
            _logger.LogInformation("Tcp bridge is not started. Starting...");

            var networkStream = _networkStreamCache.GetOrAdd(targetId);

            _tcpBridge.RunAsync(networkStream.GetStream(), networkStream.GetStream(), _cancellationTokenSource);
        }

        return await _publishingSendingOrchestrator.Execute(sendWrapper);
    }

    public void Dispose()
    {
        _tcpBridge.Dispose();
        _cancellationTokenSource.Dispose();
    }
}