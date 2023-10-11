using Domain.Common;

namespace Domain.Services.Common;

public interface IComHandler
{
    Task<Result> SendMessage(byte[] value);

    Task<Result> WaitForResponse(Func<byte[], Result<byte[]>> reminderSelector);

    IAsyncEnumerable<byte[]> GetAccumulator();
}