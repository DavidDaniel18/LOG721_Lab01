using Domain.Common;
using Domain.ProtoTransit.Entities.Interfaces;

namespace Domain.ProtoTransit;

public abstract partial class ProtoTransit : IEncodeMessage
{
    public Result<byte[]> GetBytes()
    {
        if(_bytes is not null) return Result.Success(_bytes);

        var headerBytes = Header.GetBytes();

        if (headerBytes.IsFailure()) return Result.FromFailure<byte[]>(headerBytes);

        var payloadBytesResult = GetPayloadBytes();

        if(payloadBytesResult.IsFailure()) return Result.FromFailure<byte[]>(payloadBytesResult);

        var result = new byte[headerBytes.Content!.Length + payloadBytesResult.Content!.Length];

        headerBytes.Content!.CopyTo(result, 0);

        payloadBytesResult.Content!.CopyTo(result, headerBytes.Content!.Length);

        _bytes = result;

        return Result.Success(_bytes);
    }

    private protected virtual Result<byte[]> GetPayloadBytes() => Result.Success(Array.Empty<byte>());
}