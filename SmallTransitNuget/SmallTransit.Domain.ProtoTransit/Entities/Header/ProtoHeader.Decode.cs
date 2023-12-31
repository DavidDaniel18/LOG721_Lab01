﻿using SmallTransit.Abstractions.Monads;
using SmallTransit.Domain.ProtoTransit.Exceptions;
using SmallTransit.Domain.ProtoTransit.Extensions;
using SmallTransit.Domain.ProtoTransit.Seedwork;
using SmallTransit.Domain.ProtoTransit.ValueObjects.Header;
using SmallTransit.Domain.ProtoTransit.ValueObjects.Properties;

namespace SmallTransit.Domain.ProtoTransit.Entities.Header;

internal partial class ProtoHeader
{
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

    internal Result<List<MessagePortion>> GetPropertyHeaderInfos(IEnumerable<ProtoProperty> protoProperties, int messageLength)
    {
        var messagePortions = new List<MessagePortion>();

        var totalHeaderLength = GetHeaderItemValue(typeof(HeaderLengthItem)).Content;

        var currentIndex = 0;

        foreach (var protoProperty in protoProperties)
        {
            var valueResult = GetHeaderItemValue(protoProperty.HeaderType);

            if (valueResult.IsFailure()) return Result.FromFailure<List<MessagePortion>>(valueResult);

            if(totalHeaderLength + currentIndex + valueResult.Content > messageLength) return Result.Failure<List<MessagePortion>> (new MessageIncompleteException());

            messagePortions.Add(new MessagePortion(totalHeaderLength + currentIndex, valueResult.Content, protoProperty.GetType()));

            currentIndex += valueResult.Content;
        }

        return Result.Success(messagePortions);
    }

    private protected Result<int> GetHeaderItemValue(Type type)
    {
        if (_headerItems.ContainsKey(type) is false) return Result.Failure<int>($"Header {type} not found");

        var header = _headerItems[type];

        var value = header.HeaderValue.FromBytesToInt();

        return Result.Success(value);
    }

    private protected Result InitializeSpecificHeadersFromBytes(byte[] header)
    {
        var currentIndex = HeaderLengthItem.StorageSizeInBytes + MessageTypeItem.StorageSizeInBytes;

        foreach (var protoHeaderItem in GetSpecificHeaderItems().Select(ProtoHeaderItem.Create))
        {
            protoHeaderItem.HeaderValue = header[currentIndex..(currentIndex + protoHeaderItem.HeaderLength)];

            _headerItems[protoHeaderItem.GetType()] = protoHeaderItem;

            currentIndex += protoHeaderItem.HeaderLength;
        }

        return Result.Success();
    }


    private IEnumerable<Type> GetSpecificHeaderItems()
    {
        return _headerItems
            .Where(kv => _defaultHeaderTypes.Contains(kv.Key) is false)
            .OrderBy(kv => kv.Value.Order)
            .Select(kv => kv.Key)
            .ToArray();
    }
}