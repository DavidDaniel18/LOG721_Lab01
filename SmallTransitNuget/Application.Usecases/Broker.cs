using Application.Services.InfrastructureInterfaces;
using Application.Services.Orchestrator.Receiving;
using Application.Services.Orchestrator.Sending;
using Domain.Common.Monads;
using Domain.Services.Receiving;
using Domain.Services.Receiving.BrokerReceive;
using Domain.Services.Sending.Push;
using Domain.Services.Sending.Subscribing.Dto;
using SmallTransit.Abstractions.Broker;

namespace Application.UseCases;

public sealed class Broker : IDisposable
{
    private BrokerReceiveOrchestrator? _brokerReceiveOrchestrator;
    private PushingSendingOrchestrator? _pushingSendOrchestrator;
    private readonly ITcpBridge _tcpBridge;
    private readonly IControllerDelegate<BrokerReceiveWrapper> _controllerDelegate;

    public Broker(ITcpBridge tcpBridge, IControllerDelegate<BrokerReceiveWrapper> controllerDelegate)
    {
        _tcpBridge = tcpBridge;
        _controllerDelegate = controllerDelegate;
    }

    internal async Task<Result<SubscriptionWrapper>> BeginListen(Stream inputStream, Stream outputStream, CancellationTokenSource cancellationTokenSource)
    {
        if (_brokerReceiveOrchestrator is not null) return Result.Failure<SubscriptionWrapper>("Broker is already listening");

        _tcpBridge.RunAsync(inputStream, outputStream, cancellationTokenSource);

        _brokerReceiveOrchestrator = new BrokerReceiveOrchestrator(new BrokerReceiveContext(), _tcpBridge, _controllerDelegate);

        try
        {
            var brokerReceiveOrchestrator = await _brokerReceiveOrchestrator.Execute().WaitAsync(cancellationTokenSource.Token);

            return brokerReceiveOrchestrator;
        }
        catch (Exception)
        {
            return Result.Failure<SubscriptionWrapper>("Tcp Bridge Task has finished");
        }
    }

    internal async Task<Result> Push(byte[] message)
    {
        if (_brokerReceiveOrchestrator is null) return Result.Failure("Broker has no connection");

        _pushingSendOrchestrator ??= new PushingSendingOrchestrator(new PushContext(), _tcpBridge);

        return await _pushingSendOrchestrator.Execute(new PushWrapper(message));
    }

    public void Dispose()
    {
        _tcpBridge.Dispose();
    }
}