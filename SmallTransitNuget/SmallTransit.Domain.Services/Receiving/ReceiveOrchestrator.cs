using SmallTransit.Domain.Services.Common;
using SmallTransit.Domain.Services.Receiving.States;

namespace SmallTransit.Domain.Services.Receiving;

internal abstract class ReceiveOrchestrator<TContext, TPayload, TStateResult, TResult>
    where TContext : ReceiveStateHolder<TContext, TPayload, TStateResult, TResult>
    where TStateResult : StateResult<TResult>
{
    private protected readonly IComHandler ComHandler;

    private protected abstract TContext Context { get; }

    protected ReceiveOrchestrator(IComHandler comHandler)
    {
        ComHandler = comHandler;
    }
}