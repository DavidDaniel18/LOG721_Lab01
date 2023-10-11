using Application.Services.Orchestrator;
using Domain.Common;
using Domain.Services.Send.Publishing;
using Application.Services.InfrastructureInterfaces;
using Domain.Services.Common;
using Microsoft.Extensions.Logging;

namespace Application.UseCases;

public sealed class PublishClient<TContract>
{
    private readonly ILogger<PublishClient<TContract>> _logger;
    private readonly PublishingSendOrchestrator<TContract> _publishingSendOrchestrator;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public PublishClient(ITcpBridge tcpBridge, INetworkStream networkStream, IComHandler comHandler, ILogger<PublishClient<TContract>> logger)
    {
        _logger = logger;

        _publishingSendOrchestrator = new PublishingSendOrchestrator<TContract>(new PublishContext(), comHandler);

        tcpBridge.RunAsync(networkStream.GetStream(), networkStream.GetStream(), _cancellationTokenSource);
    }

    internal async Task<Result> Publish(PublishWrapper<TContract> publishWrapper)
    {
        if(_cancellationTokenSource.IsCancellationRequested) return Result.Failure("Bridging task is completed");

        return await _publishingSendOrchestrator.Execute(publishWrapper);
    }
}