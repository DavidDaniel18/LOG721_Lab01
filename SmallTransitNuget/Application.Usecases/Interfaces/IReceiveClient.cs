using Application.Services.InfrastructureInterfaces;
using Domain.Common;
using Domain.Services.Send.Subscribing.Dto;

namespace Application.UseCases.Interfaces;

public interface IReceiveClient
{
    Task<Result> Subscribe(INetworkStream stream, SubscriptionWrapper subscriptionWrapper);

    Task<Result> BeginListen();
}