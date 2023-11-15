using Domain.Common.Monads;
using Domain.Services.Common;
using Domain.Services.Sending;
using Domain.Services.Sending.Push;

namespace Application.Services.Orchestrator.Sending;

internal sealed class PushingSendingOrchestrator : SendingOrchestrator<PushContext, byte[]>
{
    private protected override PushContext Context { get; }

    internal PushingSendingOrchestrator(PushContext context, IComHandler comHandler) : base(comHandler)
    {
        Context = context;
    }

    internal async Task<Result> Execute(PushWrapper pushWrapper) => await Send(pushWrapper.payload);
}