using Application.Services.InfrastructureInterfaces;
using Microsoft.Extensions.Logging;
using Domain.Services.Sending.Publishing;
using Application.Services.Orchestrator.Sending;
using Domain.Common.Monads;

namespace Application.UseCases;

public sealed class PublishClient<TContract> : IDisposable
{
    private readonly ITcpBridge _tcpBridge;
    private readonly INetworkStreamCache _networkStreamCache;
    private readonly ILogger<PublishClient<TContract>> _logger;
    private readonly PublishingSendingOrchestrator<TContract> _publishingSendingOrchestrator;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public PublishClient(ITcpBridge tcpBridge, INetworkStreamCache networkStreamCache, ILogger<PublishClient<TContract>> logger)
    {
        _tcpBridge = tcpBridge;
        _networkStreamCache = networkStreamCache;
        _logger = logger;

        _publishingSendingOrchestrator = new PublishingSendingOrchestrator<TContract>(new PublishContext(), _tcpBridge);
    }

    internal async Task<Result> Publish(PublishWrapper<TContract> publishWrapper, string brokerKey)
    {
        if(_tcpBridge.IsCompleted()) return Result.Failure("Bridging task is completed");

        if (_tcpBridge.IsNotStarted())
        {
            var networkStream = _networkStreamCache.Get(brokerKey);

            _tcpBridge.RunAsync(networkStream.GetStream(), networkStream.GetStream(), _cancellationTokenSource);
        }

        return await _publishingSendingOrchestrator.Execute(publishWrapper);
    }

    public void Dispose()
    {
        _tcpBridge.Dispose();
        _cancellationTokenSource.Dispose();
    }
}