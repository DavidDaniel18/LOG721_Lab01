using Domain.Common;

namespace SmallTransit.Abstractions.Interfaces;

public interface IBrokerPushEndpoint
{
    Task<Result> Push(byte[] message);
}