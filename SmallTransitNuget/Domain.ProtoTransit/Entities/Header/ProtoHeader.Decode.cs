using Domain.Common;
using Domain.Common.Exceptions;
using Domain.ProtoTransit.Extensions;
using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.Entities.Header;

internal partial class ProtoHeader
{
    private readonly List<Type> _specificHeaderItems = new ();

    private protected void AddSpecificHeaderItem<THeaderItem>() where THeaderItem : ProtoHeaderItem
        => _specificHeaderItems.Add(typeof(THeaderItem));

    private protected int GetTotalHeaderLength() => Convert.ToInt32(_headerItems[typeof(HeaderLengthItem)].HeaderValue.FromBytesToInt());

    private protected Result<int> GetHeaderValue<THeaderItem>() where THeaderItem : ProtoHeaderItem
    {
        var type = typeof(THeaderItem);

        if (_headerItems.ContainsKey(type) is false) return Result.Failure<int>(new DomainGenericException($"Header {typeof(THeaderItem)} not found"));

        var header = _headerItems[typeof(THeaderItem)];

        var value = header.HeaderValue.FromBytesToInt();

        return Result.Success(Convert.ToInt32(value));
    }

    internal Result InitializeFromBytes(byte[] message)
    {
        var headerLengthValueInBytes = message[..HeaderLengthItem.StorageSizeInBytes];

        _headerItems[typeof(HeaderLengthItem)].HeaderValue = headerLengthValueInBytes;

        var messageTypeBytes = message[HeaderLengthItem.StorageSizeInBytes..(HeaderLengthItem.StorageSizeInBytes + HeaderLengthItem.StorageSizeInBytes)];

        _headerItems[typeof(MessageTypeItem)].HeaderValue = messageTypeBytes;

        return InitializeSpecificHeadersFromBytes(message);
    }

    internal static Result<MessageTypesEnum> ParseMessageTypeFromBytes(byte[] header)
    {
        var messageTypeBytes = header[HeaderLengthItem.StorageSizeInBytes..(HeaderLengthItem.StorageSizeInBytes + HeaderLengthItem.StorageSizeInBytes)];

        var messageType = (MessageTypesEnum)messageTypeBytes.FromBytesToInt();

        return Result.Success(messageType);
    }

    private protected virtual Result InitializeSpecificHeadersFromBytes(byte[] header)
    {
        var currentIndex = HeaderLengthItem.StorageSizeInBytes + MessageTypeItem.StorageSizeInBytes;

        foreach (var protoHeaderItem in _specificHeaderItems.Select(ProtoHeaderItem.Create))
        {
            protoHeaderItem.HeaderValue = header[currentIndex..(currentIndex + protoHeaderItem.HeaderLength)];
            
            AddHeaderItem(protoHeaderItem);

            currentIndex += protoHeaderItem.HeaderLength;
        }

        return Result.Success();
    }
}