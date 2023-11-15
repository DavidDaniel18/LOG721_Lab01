using Domain.ProtoTransit;
using Domain.Services.Common;
using Domain.Services.Sending.Subscribing.Dto;

namespace Domain.Services.Receiving.States;

internal record StateResult<TResult>(Protocol Response)
{
    internal TResult? Result { get; init; }

    internal SubscriptionDto? SubscriptionDto { get; init; }

    internal ContextAlterationRequests? ContextType { get; init; }

    internal bool ShouldReturn => ContextType is not null || SubscriptionDto is not null;
}