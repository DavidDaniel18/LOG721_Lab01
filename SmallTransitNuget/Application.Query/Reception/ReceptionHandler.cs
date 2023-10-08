using Application.Common.Interfaces;
using Domain.Common;

namespace Application.Queries.Reception;

internal sealed class ReceptionHandler<TContract> : IHandler<TContract, Receive> where TContract : class
{
    public Task<Result<TContract>> HandleAsync(Receive request)
    {
        throw new NotImplementedException();
    }
}