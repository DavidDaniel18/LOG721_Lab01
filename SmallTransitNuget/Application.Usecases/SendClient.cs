using Application.Services.InfrastructureInterfaces;
using Application.Services.Orchestrator.Sending;
using Domain.Common.Monads;
using Domain.Services.Sending.Send;
using Microsoft.Extensions.Logging;

namespace Application.UseCases;

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
            var networkStream = _networkStreamCache.Get(targetId);

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