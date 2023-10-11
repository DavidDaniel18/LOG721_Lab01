using SmallTransit.Application.Common;
using SmallTransit.Application.Interfaces;

namespace SmallTransit.Application.Requests.Reception;

internal sealed class ReceptionHandler<TContract> : IHandler<TContract, Receive> where TContract : class
{
    public Task<Result<TContract>> HandleAsync(Receive request)
    {
        throw new NotImplementedException();
    }
}