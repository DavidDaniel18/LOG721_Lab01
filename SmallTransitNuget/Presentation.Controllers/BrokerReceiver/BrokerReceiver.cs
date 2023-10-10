using Application.Common.Broker;
using Application.Common.Interfaces;
using Application.UseCases;
using Domain.Common;
using Domain.Services.Receive;
using Microsoft.AspNetCore.Connections;

namespace Presentation.Controllers.BrokerReceiver;

public sealed class BrokerReceiver : ConnectionHandler, IBrokerPushEndpoint
{
    private readonly Broker _broker;
    private readonly IControllerDelegate<BrokerSubscriptionDto> _controllerDelegate;

    internal BrokerReceiver(Broker broker, IControllerDelegate<BrokerSubscriptionDto> controllerDelegate)
    {
        _broker = broker;
        _controllerDelegate = controllerDelegate;
    }

    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        await Receive(connection, new CancellationTokenSource());
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