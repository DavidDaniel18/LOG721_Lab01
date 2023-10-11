using Domain.Common;
using Domain.Services.Common;
using Domain.Services.Send;
using Domain.Services.Send.Push;

namespace Application.Services.Orchestrator;

internal sealed class PushingSendOrchestrator : SendOrchestrator<PushContext, byte[]>
{
    private protected override PushContext Context { get; }

    internal PushingSendOrchestrator(PushContext context, IComHandler comHandler) : base(comHandler)
    {
        Context = context;
    }

    internal async Task<Result> Execute(PushWrapper pushWrapper) => await Send(pushWrapper.payload);
}