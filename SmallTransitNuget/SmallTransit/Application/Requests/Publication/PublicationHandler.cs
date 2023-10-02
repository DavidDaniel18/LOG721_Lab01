using SmallTransit.Application.Common;
using SmallTransit.Application.Interfaces;

namespace SmallTransit.Application.Requests.Publication;

internal sealed class PublicationHandler : IHandler<Publish>
{
    internal PublicationHandler(public)
    {
        
    }

    public Task<Result> HandleAsync(Publish request)
    {
        throw new NotImplementedException();
    }
}