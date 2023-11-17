using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Application.Services.InfrastructureInterfaces;
using SmallTransit.Application.Services.Orchestrator.Receiving;
using SmallTransit.Application.Services.Orchestrator.Sending;
using SmallTransit.Domain.Services.Receiving;
using SmallTransit.Domain.Services.Receiving.BrokerReceive;
using SmallTransit.Domain.Services.Sending.Push;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;

namespace SmallTransit.Application.UseCases;

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