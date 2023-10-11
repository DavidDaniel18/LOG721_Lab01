using Application.Services.InfrastructureInterfaces;
using Application.Services.Orchestrator;
using Application.UseCases.Interfaces;
using Domain.Common;
using Domain.Services.Common;
using Domain.Services.Receive;
using Domain.Services.Receive.ClientReceive;
using Domain.Services.Send.Subscribing;
using Domain.Services.Send.Subscribing.Dto;

namespace Application.UseCases;

public sealed class ReceiveClient<TContract> : IReceiveClient
{
    private SubscribingOrchestrator? _subscribingOrchestrator;
    private ClientReceiveOrchestrator<TContract>? _clientReceiveOrchestrator;
    private readonly ITcpBridge _tcpBridge;
    private readonly IComHandler _comHandler;
    private readonly IControllerDelegate<TContract> _controllerDelegate;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public ReceiveClient(ITcpBridge tcpBridge, IComHandler comHandler, IControllerDelegate<TContract> controllerDelegate) 
    {
        _tcpBridge = tcpBridge;
        _comHandler = comHandler;
        _controllerDelegate = controllerDelegate;
    }

    public async Task<Result> Subscribe(INetworkStream stream, SubscriptionWrapper subscriptionWrapper)
    {
        if(_subscribingOrchestrator is not null) return Result.Failure("Already subscribed");

        _tcpBridge.RunAsync(stream.GetStream(), stream.GetStream(), _cancellationTokenSource);

        _subscribingOrchestrator = new SubscribingOrchestrator(new SubscribeContext(), _comHandler);

        try
        {
            var subscribing = await _subscribingOrchestrator.Execute(subscriptionWrapper).WaitAsync(_cancellationTokenSource.Token);

            return subscribing;
        }
        catch (Exception)
        {
            return Result.Failure<SubscriptionWrapper>("Tcp Bridge Task has finished");
        }
    }

    public async Task<Result> BeginListen()
    {
        if (_cancellationTokenSource.IsCancellationRequested) return Result.Failure("Bridging task is completed");

        if (_clientReceiveOrchestrator is not null) return Result.Failure("Already listening");

        _clientReceiveOrchestrator = new ClientReceiveOrchestrator<TContract>(new ClientReceiveContext(), _comHandler, _controllerDelegate);

        return await _clientReceiveOrchestrator.Execute();
    }
}