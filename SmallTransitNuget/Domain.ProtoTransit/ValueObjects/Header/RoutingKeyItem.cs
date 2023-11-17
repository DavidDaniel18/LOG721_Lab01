namespace SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

public sealed record RoutingKeyItem() : ProtoHeaderItem("RoutingKeyItem", StorageSizeInBytes, 3)
{
    internal static readonly int StorageSizeInBytes = 1;
}