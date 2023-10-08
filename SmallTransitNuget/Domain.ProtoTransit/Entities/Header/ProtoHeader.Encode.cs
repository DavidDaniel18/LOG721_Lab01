using Domain.Common;
using Domain.ProtoTransit.Extensions;
using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.Entities.Header;

internal partial class ProtoHeader : IEncodeHeader
{
    private byte[]? _bytes;

    public Result<byte[]> GetBytes()
    {
        if (_bytes is not null) return Result.Success(_bytes);

        var headerBytesResult = GetHeaderBytes();

        if (headerBytesResult.IsFailure()) return headerBytesResult;

        _bytes = headerBytesResult.Content;

        return headerBytesResult;
    }

    private Result<byte[]> GetHeaderBytes()
    {
        var totalHeaderLength = _headerItems.Values.Sum((a) => a.HeaderLength) + HeaderLengthItem.StorageSizeInBytes;

        _headerItems[typeof(HeaderLengthItem)].HeaderValue = totalHeaderLength.ToBytes();

        var header = new byte[totalHeaderLength];

        var currentIndex = 0;

        foreach (var headerItem in _headerItems.Values.OrderBy(hi => hi.Order))
        {
            var headerBytes = headerItem.HeaderValue;

            headerBytes.CopyTo(header, currentIndex);

            currentIndex += headerItem.HeaderLength;
        }

        return Result.Success(header);
    }
}