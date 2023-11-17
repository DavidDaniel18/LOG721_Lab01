using SmallTransit.Abstractions.Monads;

namespace Interfaces
{
    public interface IBrokerChannelListener
    {
        Task<Result> Listen();
    }
}
