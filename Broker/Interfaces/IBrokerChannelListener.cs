using Domain.Common;

namespace Interfaces
{
    public interface IBrokerChannelListener
    {
        Task<Result> Listen();
    }
}
