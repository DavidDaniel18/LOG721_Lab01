namespace Domain.ProtoTransit.ValueObjects.Header;

public sealed record HeaderLengthItem() : ProtoHeaderItem("HeaderLengthItem", StorageSizeInBytes, 0)
{
    internal static readonly int StorageSizeInBytes = 1;
}