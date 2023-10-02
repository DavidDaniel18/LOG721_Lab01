using Domain.ProtoTransit.ValueObjects.Header;

namespace Domain.ProtoTransit.Entities.Header;

internal partial class ProtoHeader
{
    private readonly Dictionary<Type, ProtoHeaderItem> _headerItems = new()
    {
        { typeof(HeaderLengthItem), new HeaderLengthItem() },
        { typeof(MessageTypeItem), new MessageTypeItem() }
    };

    private protected static readonly int BaseHeaderLength = MessageTypeItem.StorageSizeInBytes + MessageTypeItem.StorageSizeInBytes;

    internal MessageTypesEnum MessageType { get; }

    internal ProtoHeader(MessageTypesEnum messageType)
    {
        MessageType = messageType;
    }

    private ProtoHeader(MessageTypesEnum messageType, Dictionary<Type, ProtoHeaderItem> headerItems)
    {
        MessageType = messageType;
        _headerItems = headerItems;
    }

    private protected void AddHeaderItem<THeaderItem>(THeaderItem headerItem) where THeaderItem : ProtoHeaderItem
    {
        _headerItems.Add(typeof(THeaderItem), headerItem);
    }
}