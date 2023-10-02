namespace Presentation.Controllers;

public sealed class Publisher
{
    public Task<Result> Publish<TContract>(Payload<TContract> payload) where TContract : class
    {
        return Task.FromResult(Result.Success());
    }
}