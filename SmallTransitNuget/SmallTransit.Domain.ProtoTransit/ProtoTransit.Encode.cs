using SmallTransit.Abstractions.Monads;

namespace SmallTransit.Domain.ProtoTransit;

internal abstract partial class Protocol
{
    public Result<byte[]> GetBytes()
    {
        if (_bytes is not null) return Result.Success(_bytes);

        return Header.GetBytes()
            .Bind(headerBytes => GetBodyBytes(_protoProperties.Values.Select(prop => prop.Bytes).ToList())
                .Bind(payloadBytes =>
                {
                    var result = new byte[headerBytes.Length + payloadBytes.Length];

                    headerBytes.CopyTo(result, 0);

                    payloadBytes.CopyTo(result, headerBytes.Length);

                    _bytes = result;

                    return Result.Success(_bytes);
                }));
    }

    private Result<byte[]> GetBodyBytes(List<byte[]> properties)
    {
        if (properties.Any(prop => prop.Length < 1))
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