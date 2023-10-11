namespace Domain.ProtoTransit.Seedwork;

internal struct MessagePortion
{
    internal readonly Type PropertyType;
    internal readonly int BeginAtIndex;
    internal readonly int Length;

    internal MessagePortion(int beginAtIndex, int length, Type propertyType)
    {
        BeginAtIndex = beginAtIndex;
        Length = length;
        PropertyType = propertyType;
    }
}