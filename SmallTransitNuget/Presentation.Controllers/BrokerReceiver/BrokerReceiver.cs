using Application.UseCases;
using Domain.Common.Monads;
using Domain.Services.Receiving;
using Microsoft.AspNetCore.Connections;
using SmallTransit.Abstractions.Broker;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.BrokerReceiver;

public sealed class BrokerReceiver : IBrokerPushEndpoint
{
    private readonly Broker _broker;
    private readonly IControllerDelegate<BrokerSubscriptionDto> _controllerDelegate;

    public BrokerReceiver(Broker broker, IControllerDelegate<BrokerSubscriptionDto> controllerDelegate)
    {
        _broker = broker;
        _controllerDelegate = controllerDelegate;
    }

    public async Task<Result> Receive(ConnectionContext connectionContext, CancellationTokenSource cancellationTokenSource)
    {
        var listeningResult = await _broker.BeginListen(connectionContext.Transport.Input.AsStream(), connectionContext.Transport.Output.AsStream(), cancellationTokenSource);

        if (listeningResult.IsSuccess())
        {
            await _controllerDelegate.SendToController(new BrokerSubscriptionDto(
                listeningResult.Content!.RoutingKey,
                listeningResult.Content!.PayloadType,
                listeningResult.Content!.QueueName,
                this));

            while (cancellationTokenSource.IsCancellationRequested is false)
            {
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }

        return Result.FromFailure(listeningResult);
    }

    public async Task<Result> Push(byte[] message)
    {
        return await _broker.Push(message);
    }
}