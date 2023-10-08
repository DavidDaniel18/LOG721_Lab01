using Application.Common.Interfaces;
using Domain.Common;

namespace Application.Commands.Publication;

internal sealed class PublicationHandler : IHandler<Publish>
{
    internal PublicationHandler()
    {
        
    }

    public Task<Result> HandleAsync(Publish request)
    {
        
    }
}