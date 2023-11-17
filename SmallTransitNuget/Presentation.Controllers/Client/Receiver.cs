using Microsoft.AspNetCore.Connections;
using SmallTransit.Abstractions.Monads;
using SmallTransit.Application.UseCases;
using SmallTransit.Application.UseCases.Interfaces;

namespace SmallTransit.Presentation.Controllers.Client;

public sealed class Receiver
{
    private readonly IReceivePoint _receivePoint;

    public Receiver(ReceivePoint receivePoint)
    {
        _receivePoint = receivePoint;
    }

    public async Task<Result> Receive(ConnectionContext connectionContext, CancellationTokenSource cancellationTokenSource)
    {
        return await _receivePoint.BeginListen(connectionContext.Transport.Input.AsStream(), connectionContext.Transport.Output.AsStream(), cancellationTokenSource);
    }
}