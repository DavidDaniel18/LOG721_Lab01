using Domain.Common;
using MessagePack;

namespace Domain.Services.Send.Push;

internal sealed class PushingSendOrchestrator : SendOrchestrator<PushContext, byte[]>
{
    private protected override PushContext Context { get; }

    internal PushingSendOrchestrator(PushContext context, IComHandler comHandler) : base(comHandler)
    {
        Context = context;
    }

    internal async Task<Result> Execute(PushWrapper pushWrapper) => await Serialize(pushWrapper).BindAsync(Send);

    private static Result<byte[]> Serialize(PushWrapper pushWrapper)
    {
        try
        {
            return Result.Success(MessagePackSerializer.Serialize(pushWrapper.payload));
        }
        catch (Exception e)
        {
            return Result.Failure<byte[]>(e);
        }
    }
}