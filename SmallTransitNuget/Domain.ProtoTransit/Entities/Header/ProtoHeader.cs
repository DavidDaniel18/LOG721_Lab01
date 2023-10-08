using Domain.Common;
using Domain.ProtoTransit.Extensions;
using Domain.ProtoTransit.ValueObjects.Header;
using Domain.ProtoTransit.ValueObjects.Properties;

namespace Domain.ProtoTransit.Entities.Header;

internal partial class ProtoHeader
{
    private readonly HashSet<Type> _defaultHeaderTypes = new() { typeof(HeaderLengthItem), typeof(MessageTypeItem) };

    private readonly Dictionary<Type, ProtoHeaderItem> _headerItems = new();

    private MessageTypesEnum MessageType { get; }

    internal ProtoHeader(MessageTypesEnum messageType)
    {
        MessageType = messageType;

        foreach (var defaultHeaderType in _defaultHeaderTypes)
        {
            _headerItems.Add(defaultHeaderType, ProtoHeaderItem.Create(defaultHeaderType));
        }

        _headerItems[typeof(MessageTypeItem)].HeaderValue = ((int)MessageType).ToBytes();
    }

    private void AddHeaderItem<THeaderItem>(THeaderItem headerItem) where THeaderItem : ProtoHeaderItem
    {
        _headerItems.Add(typeof(THeaderItem), headerItem);
    }

    private Result AddHeaderItem(Type type)
    {
        if(type.IsSubclassOf(typeof(ProtoHeader)) ) return Result.Failure($"Header {type} is not a valid header");

        AddHeaderItem(ProtoHeaderItem.Create(type));

        return Result.Success();
    }

    private protected void AddHeaderItem<THeaderItem>() where THeaderItem : ProtoHeaderItem
    {
        AddHeaderItem(ProtoHeaderItem.Create(typeof(THeaderItem)));
    }

    internal Result TrySetValue(Type headerType, byte[] value)
    {
        return _headerItems.TryGetValue(headerType, out var headerItem) ?
        Result.Success(headerItem.HeaderValue = value.Length.ToBytes()) :
        Result.Failure($"Header {headerType} is not a valid header");
    }

    internal Result SetHeaders(IEnumerable<ProtoProperty> getProperties)
    {
        foreach (var protoProperty in getProperties)
        {
            var result = AddHeaderItem(protoProperty.HeaderType);

            if (result.IsFailure()) return result;
        }

        return Result.Success();
    }
}