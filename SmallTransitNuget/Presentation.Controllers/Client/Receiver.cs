using Application.UseCases.Interfaces;
using Domain.Common.Monads;
using Microsoft.AspNetCore.Connections;

namespace Presentation.Controllers.Client;

public sealed class Receiver
{
    private readonly IReceivePoint _receivePoint;

    public Receiver(IReceivePoint receivePoint)
    {
        _receivePoint = receivePoint;
    }

    public async Task<Result> Receive(ConnectionContext connectionContext, CancellationTokenSource cancellationTokenSource)
    {
        return await _receivePoint.BeginListen(connectionContext.Transport.Input.AsStream(), connectionContext.Transport.Output.AsStream(), cancellationTokenSource);
    }
}