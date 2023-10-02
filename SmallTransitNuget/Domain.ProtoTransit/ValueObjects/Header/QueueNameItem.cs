namespace Domain.ProtoTransit.ValueObjects.Header;

internal sealed record QueueNameItem(int Order = 5) : ProtoHeaderItem("QueueNameItem", StorageSizeInBytes, Order)
{
    internal static readonly int StorageSizeInBytes = 1;
}