using SmallTransit.Abstractions.Monads;
using SmallTransit.Application.Services.InfrastructureInterfaces;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;

namespace SmallTransit.Application.UseCases.Interfaces;

public interface IReceiveSubscriberClient : IDisposable
{
    Task<Result> Subscribe(INetworkStream stream, SubscriptionWrapper subscriptionWrapper);

    Task<Result> BeginListen();
}