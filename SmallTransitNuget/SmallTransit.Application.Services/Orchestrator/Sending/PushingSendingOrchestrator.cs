using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Sending;
using SmallTransit.Domain.Services.Sending.Push;

namespace SmallTransit.Application.Services.Orchestrator.Sending;

internal sealed class PushingSendingOrchestrator : SendingOrchestrator<PushContext, byte[]>
{
    private protected override PushContext Context { get; }

    internal PushingSendingOrchestrator(PushContext context, IComHandler comHandler) : base(comHandler)
    {
        Context = context;
    }

    internal async Task<Result> Execute(PushWrapper pushWrapper) => await Send(pushWrapper.payload);
}