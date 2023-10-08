using Domain.ProtoTransit;
using Domain.Services.Common;

namespace Domain.Services.Receive.States;

internal record StateResult<TResult>(Protocol Response)
{
    internal TResult? Result { get; init; }

    internal ContextAlterationRequests? ContextType { get; init; }

    internal bool ShouldReturn => ContextType is not null;
}