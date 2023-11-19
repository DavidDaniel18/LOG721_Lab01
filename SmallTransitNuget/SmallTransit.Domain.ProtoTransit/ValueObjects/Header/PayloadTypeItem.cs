namespace SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

public sealed record PayloadTypeItem() : ProtoHeaderItem("PayloadTypeItem", StorageSizeInBytes, 4)
{
    internal static readonly int StorageSizeInBytes = 1;
}