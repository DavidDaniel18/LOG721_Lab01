using Domain.Common;
using Microsoft.AspNetCore.Connections;
using Presentation.Controllers.Intefaces;

namespace Presentation.Controllers.BrokerReceiver;

public interface IBrokerReceiveEndpoint
{
    Task<Result<IBrokerPushEndpoint>> Receive(ConnectionContext connectionContext, CancellationTokenSource cancellationTokenSource);
}