namespace Application.Commands.Publication;

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