using Domain.Common;
using Domain.ProtoTransit.Entities.Interfaces;
using Domain.ProtoTransit.Extensions;
using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.Entities.Header;

internal partial class ProtoHeader : IEncodeHeader
{
    private byte[]? _bytes;

    public Result<byte[]> GetBytes()
    {
        return _bytes is null ? GetHeaderBytes() : Result.Success(_bytes);
    }

    

    private Result<byte[]> GetHeaderBytes()
    {
        AddMessageTypeItem();

        ComputeHeaderLength();

        var header = new byte[_headerItems[typeof(HeaderLengthItem)].HeaderValue.Value.Length];

        var currentIndex = 0;

        foreach (var headerItem in _headerItems.Values.OrderBy(hi => hi.Order))
        {
            var headerBytes = headerItem.HeaderValue.Value;

            headerBytes.CopyTo(header, currentIndex);

            currentIndex += headerItem.HeaderLength;
        }

        return Result.Success(header);
    }

    private void ComputeHeaderLength()
    {
        var totalHeaderLength = _headerItems.Values.Sum((a) => a.HeaderLength) + HeaderLengthItem.StorageSizeInBytes;

        _headerItems.Add(typeof(HeaderLengthItem), new HeaderLengthItem(new Lazy<byte[]>(() => totalHeaderLength.ToBytes())));
    }

    private void AddMessageTypeItem()
    {
        _headerItems.Add(typeof(MessageTypeItem), new MessageTypeItem(new(((int)MessageType).ToBytes())));
    }
}