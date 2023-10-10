using Application.Services.Orchestrator;
using Domain.Common;
using Domain.Services.Send.Publishing;
using Application.Services.InfrastructureInterfaces;
using Domain.Services.Common;
using Infrastructure.TcpClient;

namespace Application.UseCases;

internal sealed class PublishClient<TContract>
{
    private readonly PublishingSendOrchestrator<TContract> _publishingSendOrchestrator;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Task _bridgingTask;

    internal PublishClient(ITcpBridge tcpBridge, INetworkStream networkStream, IComHandler comHandler)
    {
        _publishingSendOrchestrator = new PublishingSendOrchestrator<TContract>(new PublishContext(), comHandler);
        
        _bridgingTask = tcpBridge.RunAsync(networkStream.GetStream(), networkStream.GetStream(), _cancellationTokenSource);
    }

    internal async Task<Result> Publish(PublishWrapper<TContract> publishWrapper)
    {
        if(_bridgingTask.IsCompleted) return Result.Failure("Bridging task is completed");

        return await _publishingSendOrchestrator.Execute(publishWrapper);
    }
}