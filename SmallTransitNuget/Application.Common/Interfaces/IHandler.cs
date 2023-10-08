using Domain.Common;

namespace Application.Common.Interfaces;

public interface IHandler<TResult, in TRequest> where TResult : class
{
    Task<Result<TResult>> HandleAsync(TRequest request);
}

public interface IHandler<in TRequest>
{
    Task<Result> HandleAsync(TRequest request);
}