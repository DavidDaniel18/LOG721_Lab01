using Application.Common;
using Application.Common.Interfaces;
using Domain.Common;

namespace Application.UseCases;

internal sealed class Write<T> : IHandler<Result, Payload<T>> where T : class
{
    internal Write(IApplication)
    {
        
    }

    public Task<Result<Result>> HandleAsync(Payload<T> request)
    {
        
    }
}