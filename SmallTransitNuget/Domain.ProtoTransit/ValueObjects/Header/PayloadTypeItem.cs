namespace Domain.ProtoTransit.ValueObjects.Header;

internal sealed record PayloadTypeItem(int Order = 4) : ProtoHeaderItem("PayloadTypeItem", StorageSizeInBytes, Order)
{
    internal static readonly int StorageSizeInBytes = 1;
}