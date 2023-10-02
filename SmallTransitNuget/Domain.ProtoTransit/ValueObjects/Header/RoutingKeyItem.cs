namespace Domain.ProtoTransit.ValueObjects.Header;

internal sealed record RoutingKeyItem(int Order = 3) : ProtoHeaderItem("RoutingKeyItem", StorageSizeInBytes, Order)
{
    internal static readonly int StorageSizeInBytes = 1;
}