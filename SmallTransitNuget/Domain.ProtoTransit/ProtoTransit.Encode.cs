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

        var payloadBytesResult = GetPayloadBytes(PayloadProperties);

        if(payloadBytesResult.IsFailure()) return Result.FromFailure<byte[]>(payloadBytesResult);

        var result = new byte[headerBytes.Content!.Length + payloadBytesResult.Content!.Length];

        headerBytes.Content!.CopyTo(result, 0);

        payloadBytesResult.Content!.CopyTo(result, headerBytes.Content!.Length);

        _bytes = result;

        return Result.Success(_bytes);
    }

    private Result<byte[]> GetPayloadBytes(byte[]?[] properties)
    {
        if (properties.Any(prop => prop is null))
            return Result.Failure<byte[]>("No property can be null when getting payload bytes for encoding");

        var payloadLength = properties.Sum(prop => prop!.Length);

        var payloadBytes = new byte[payloadLength];

        var index = 0;

        foreach (var property in properties)
        {
            property!.CopyTo(payloadBytes, index);

            index += property.Length;
        }

        return Result.Success(payloadBytes);
    }
}