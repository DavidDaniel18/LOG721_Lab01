using Application.Services.InfrastructureInterfaces;
using Application.Services.Orchestrator.Receiving;
using Application.Services.Orchestrator.Sending;
using Application.UseCases.Interfaces;
using Domain.Common.Monads;
using Domain.Services.Receiving;
using Domain.Services.Receiving.SubscriberReceive;
using Domain.Services.Sending.Subscribing;
using Domain.Services.Sending.Subscribing.Dto;

namespace Application.UseCases;

public sealed class ReceiveSubscriber<TContract> : IReceiveSubscriberClient
{
    private SubscribingOrchestrator? _subscribingOrchestrator;
    private SubscriberReceiveOrchestrator<TContract>? _clientReceiveOrchestrator;
    private readonly ITcpBridge _tcpBridge;
    private readonly IControllerDelegate<TContract> _controllerDelegate;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public ReceiveSubscriber(ITcpBridge tcpBridge, IControllerDelegate<TContract> controllerDelegate) 
    {
        _tcpBridge = tcpBridge;
        _controllerDelegate = controllerDelegate;
    }

    public async Task<Result> Subscribe(INetworkStream stream, SubscriptionWrapper subscriptionWrapper)
    {
        if(_subscribingOrchestrator is not null) return Result.Failure("Already subscribed");

        _tcpBridge.RunAsync(stream.GetStream(), stream.GetStream(), _cancellationTokenSource);

        _subscribingOrchestrator = new SubscribingOrchestrator(new SubscribeContext(), _tcpBridge);

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

        _clientReceiveOrchestrator = new SubscriberReceiveOrchestrator<TContract>(new SubscriberReceiveContext(), _tcpBridge, _controllerDelegate);

        return await _clientReceiveOrchestrator.Execute();
    }

    public void Dispose()
    {
        _tcpBridge.Dispose();
        _cancellationTokenSource.Dispose();
    }
}