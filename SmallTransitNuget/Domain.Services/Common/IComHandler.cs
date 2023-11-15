using Domain.Common.Monads;

namespace Domain.Services.Common;

public interface IComHandler : IDisposable
{
    Task<Result> SendMessage(byte[] value);

    Task<Result> WaitForResponse(Func<byte[], Result<byte[]>> reminderSelector);

    IAsyncEnumerable<byte[]> GetAccumulator();
}