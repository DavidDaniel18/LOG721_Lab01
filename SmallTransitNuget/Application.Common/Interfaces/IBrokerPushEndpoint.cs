using Domain.Common;

namespace Application.Common.Interfaces;

public interface IBrokerPushEndpoint
{
    Task<Result> Push(byte[] message);
}