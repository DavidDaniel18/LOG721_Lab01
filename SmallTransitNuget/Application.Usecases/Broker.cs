using Application.Services.InfrastructureInterfaces;
using Application.Services.Orchestrator;
using Domain.Common;
using Domain.Services.Common;
using Domain.Services.Receive;
using Domain.Services.Receive.BrokerReceive;
using Domain.Services.Send.Push;
using Domain.Services.Send.Subscribing.Dto;
using SmallTransit.Abstractions.Broker;

namespace Application.UseCases;

internal sealed class Broker
{
    private BrokerReceiveOrchestrator? _brokerReceiveOrchestrator;
    private PushingSendOrchestrator? _pushingSendOrchestrator;
    private readonly ITcpBridge _tcpBridge;
    private readonly IComHandler _comHandler;
    private readonly IControllerDelegate<BrokerReceiveWrapper> _controllerDelegate;

    internal Broker(ITcpBridge tcpBridge, IComHandler comHandler, IControllerDelegate<BrokerReceiveWrapper> controllerDelegate)
    {
        _tcpBridge = tcpBridge;
        _comHandler = comHandler;
        _controllerDelegate = controllerDelegate;
    }

    internal async Task<Result<SubscriptionWrapper>> BeginListen(Stream inputStream, Stream outputStream, CancellationTokenSource cancellationTokenSource)
    {
        if (_brokerReceiveOrchestrator is not null) return Result.Failure<SubscriptionWrapper>("Broker is already listening");

        var tcpBridgeTask = _tcpBridge.RunAsync(inputStream, outputStream, cancellationTokenSource);

        _brokerReceiveOrchestrator = new BrokerReceiveOrchestrator(new BrokerReceiveContext(), _comHandler, _controllerDelegate);

        var brokerReceiveOrchestratorTask = _brokerReceiveOrchestrator.Execute();

        var finishedTask = await Task.WhenAny(tcpBridgeTask, brokerReceiveOrchestratorTask);

        if (finishedTask == brokerReceiveOrchestratorTask) return brokerReceiveOrchestratorTask.Result;

        return Result.Failure<SubscriptionWrapper>("Tcp Bridge Task has finished");
    }

    internal async Task<Result> Push(byte[] message)
    {
        if (_brokerReceiveOrchestrator is null) return Result.Failure("Broker has no connection");

        _pushingSendOrchestrator ??= new PushingSendOrchestrator(new PushContext(), _comHandler);

        return await _pushingSendOrchestrator.Execute(new PushWrapper(message));
    }

}