using Domain.Common;

namespace Domain.Services.Send;

internal interface IComHandler
{
    Task<Result> SendMessage(byte[] value);

    Task<Result<byte[]>> WaitForResponse(Func<byte[], Result<byte[]>> reminderSelector);
}