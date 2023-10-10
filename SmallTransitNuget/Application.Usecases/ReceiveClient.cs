using Application.Services.InfrastructureInterfaces;
using Application.Services.Orchestrator;
using Domain.Common;
using Domain.Services.Common;
using Domain.Services.Receive;
using Domain.Services.Receive.ClientReceive;
using Domain.Services.Send.Subscribing;
using Domain.Services.Send.Subscribing.Dto;
using Infrastructure.TcpClient;

namespace Application.UseCases;

internal sealed class ReceiveClient<TContract>
{
    private SubscribingOrchestrator? _subscribingOrchestrator;
    private ClientReceiveOrchestrator<TContract>? _clientReceiveOrchestrator;
    private readonly ITcpBridge _tcpBridge;
    private readonly IComHandler _comHandler;
    private readonly IControllerDelegate<TContract> _controllerDelegate;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private Task _tcpBridgeTask;

    internal ReceiveClient(ITcpBridge tcpBridge, IComHandler comHandler, IControllerDelegate<TContract> controllerDelegate) 
    {
        _tcpBridge = tcpBridge;
        _comHandler = comHandler;
        _controllerDelegate = controllerDelegate;
    }

    internal async Task<Result> Subscribe(INetworkStream stream, SubscriptionWrapper subscriptionWrapper)
    {
        if(_subscribingOrchestrator is not null) return Result.Failure("Already subscribed");

        _tcpBridgeTask = _tcpBridge.RunAsync(stream.GetStream(), stream.GetStream(), _cancellationTokenSource);

        _subscribingOrchestrator = new SubscribingOrchestrator(new SubscribeContext(), _comHandler);

        var subscribingTask = _subscribingOrchestrator.Execute(subscriptionWrapper);

        var finishedTask = await Task.WhenAny(_tcpBridgeTask, subscribingTask);

        if (finishedTask == subscribingTask) return subscribingTask.Result;

        return Result.Failure<SubscriptionWrapper>("Tcp Bridge Task has finished");
    }

    internal async Task<Result> BeginListen()
    {
        if (_tcpBridgeTask.IsCompleted) return Result.Failure("Bridging task is completed");

        if (_clientReceiveOrchestrator is not null) return Result.Failure("Already listening");

        _clientReceiveOrchestrator = new ClientReceiveOrchestrator<TContract>(new ClientReceiveContext(), _comHandler, _controllerDelegate);

        return await _clientReceiveOrchestrator.Execute();
    }
}