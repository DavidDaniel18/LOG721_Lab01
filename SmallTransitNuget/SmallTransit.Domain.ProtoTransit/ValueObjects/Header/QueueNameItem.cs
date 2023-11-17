namespace SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

public sealed record QueueNameItem() : ProtoHeaderItem("QueueNameItem", StorageSizeInBytes, 5)
{
    internal static readonly int StorageSizeInBytes = 1;
}