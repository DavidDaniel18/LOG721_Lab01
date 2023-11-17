using SmallTransit.Abstractions.Monads;
using SmallTransit.Application.Services.InfrastructureInterfaces;
using SmallTransit.Application.Services.Orchestrator.Receiving;
using SmallTransit.Application.Services.Orchestrator.Sending;
using SmallTransit.Application.UseCases.Interfaces;
using SmallTransit.Domain.Services.Receiving;
using SmallTransit.Domain.Services.Receiving.SubscriberReceive;
using SmallTransit.Domain.Services.Sending.Subscribing;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;

namespace SmallTransit.Application.UseCases;

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