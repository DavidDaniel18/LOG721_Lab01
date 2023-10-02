namespace Domain.ProtoTransit.ValueObjects.Header;

internal sealed record PayloadSizeItem(int Order = 5) : ProtoHeaderItem("PayloadSizeItem", StorageSizeInBytes, Order)
{
    internal static readonly int StorageSizeInBytes = 2;
}