using SmallTransit.Abstractions.Monads;

namespace SmallTransit.Abstractions.Interfaces;

public interface ISender<in TContract, TResult> where TContract : class
{
    Task<Result<TResult>> Send(TContract payload, string senderId, string targetKey);
}