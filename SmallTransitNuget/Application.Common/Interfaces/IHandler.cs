namespace Application.Common.Interfaces;

internal interface IHandler<TResult, in TRequest> where TResult : class
{
    Task<Result<TResult>> HandleAsync(TRequest request);
}

internal interface IHandler<in TRequest>
{
    Task<Result> HandleAsync(TRequest request);
}