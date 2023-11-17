namespace SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

public sealed record PayloadSizeItem() : ProtoHeaderItem("PayloadSizeItem", StorageSizeInBytes, 5)
{
    internal static readonly int StorageSizeInBytes = 2;
}