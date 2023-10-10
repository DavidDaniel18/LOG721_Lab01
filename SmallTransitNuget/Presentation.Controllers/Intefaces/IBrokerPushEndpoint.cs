using Domain.Common;

namespace Presentation.Controllers.Intefaces;

public interface IBrokerPushEndpoint
{
    Task<Result> Push(byte[] message);
}