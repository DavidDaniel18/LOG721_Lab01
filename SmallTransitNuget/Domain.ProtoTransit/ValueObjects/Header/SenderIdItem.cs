namespace SmallTransit.Domain.ProtoTransit.ValueObjects.Header;

public sealed record SenderIdItem() : ProtoHeaderItem("SenderIdItem", StorageSizeInBytes, 3)
{
    internal static readonly int StorageSizeInBytes = 1;
}