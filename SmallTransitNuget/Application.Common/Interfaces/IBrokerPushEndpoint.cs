using Domain.Common.Monads;

namespace SmallTransit.Abstractions.Interfaces;

public interface IBrokerPushEndpoint
{
    Task<Result> Push(byte[] message);
}