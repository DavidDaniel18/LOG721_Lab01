using SmallTransit.Abstractions.Monads;
using SmallTransit.Application.Services.InfrastructureInterfaces;
using SmallTransit.Application.Services.Orchestrator.Receiving;
using SmallTransit.Application.UseCases.Interfaces;
using SmallTransit.Domain.Services.Receiving;
using SmallTransit.Domain.Services.Receiving.ClientReceive;

namespace SmallTransit.Application.UseCases;

public sealed class ReceivePoint : IReceivePoint
{
    private PointReceiveOrchestrator? _clientReceiveOrchestrator;
    private readonly ITcpBridge _tcpBridge;
    private readonly IReceiveControllerDelegate _controllerDelegate;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public ReceivePoint(ITcpBridge tcpBridge, IReceiveControllerDelegate controllerDelegate) 
    {
        _tcpBridge = tcpBridge;
        _controllerDelegate = controllerDelegate;
    }

    public async Task<Result> BeginListen()
    {
        if (_tcpBridge.IsCompleted()) return Result.Failure("Bridging task is completed");

        if (_clientReceiveOrchestrator is not null) return Result.Failure("Already listening");

        _clientReceiveOrchestrator = new PointReceiveOrchestrator(new ClientReceiveContext(), _tcpBridge, _controllerDelegate);

        return await _clientReceiveOrchestrator.Execute();
    }

    public async Task<Result> BeginListen(Stream inputStream, Stream outputStream, CancellationTokenSource cancellationTokenSource)
    {
        if (_tcpBridge.IsCompleted()) return Result.Failure("Bridging task is completed");

        if (_clientReceiveOrchestrator is not null) return Result.Failure("Already listening");

        if (_tcpBridge.IsNotStarted())
        {
            _tcpBridge.RunAsync(inputStream, outputStream, _cancellationTokenSource);
        }

        _clientReceiveOrchestrator = new PointReceiveOrchestrator(new ClientReceiveContext(), _tcpBridge, _controllerDelegate);

        return await _clientReceiveOrchestrator.Execute();
    }

    public void Dispose()
    {
        _tcpBridge.Dispose();
        _cancellationTokenSource.Dispose();
    }
}