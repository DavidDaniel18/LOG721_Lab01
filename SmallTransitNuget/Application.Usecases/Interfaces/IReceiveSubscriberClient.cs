using Application.Services.InfrastructureInterfaces;
using Domain.Common.Monads;
using Domain.Services.Sending.Subscribing.Dto;

namespace Application.UseCases.Interfaces;

public interface IReceiveSubscriberClient : IDisposable
{
    Task<Result> Subscribe(INetworkStream stream, SubscriptionWrapper subscriptionWrapper);

    Task<Result> BeginListen();
}