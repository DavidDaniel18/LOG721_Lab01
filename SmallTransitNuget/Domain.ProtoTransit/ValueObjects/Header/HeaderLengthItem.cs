namespace Domain.ProtoTransit.ValueObjects.Header;

internal sealed record HeaderLengthItem() : ProtoHeaderItem("HeaderLengthItem", StorageSizeInBytes, 0)
{
    internal static readonly int StorageSizeInBytes = 1;
}