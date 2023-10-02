namespace Domain.ProtoTransit.ValueObjects.Header;

internal sealed record MessageTypeItem() : ProtoHeaderItem("MessageTypeItem", StorageSizeInBytes, 1)
{
    internal static readonly int StorageSizeInBytes = 1;
}