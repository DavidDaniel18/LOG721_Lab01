namespace Domain.ProtoTransit.Seedwork;

internal struct MessagePortion
{
    internal readonly int BeginAtIndex;
    internal readonly int Length;

    internal MessagePortion(int beginAtIndex, int length)
    {
        BeginAtIndex = beginAtIndex;
        Length = length;
    }
}