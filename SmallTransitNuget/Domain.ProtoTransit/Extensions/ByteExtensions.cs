namespace Domain.ProtoTransit.Extensions;

internal static class ByteExtensions
{
    internal static byte[] ToBytes(this int value)
    {
        var bytes = BitConverter.GetBytes(value);

        ToBigEndian(bytes);

        return bytes;
    }

    internal static int FromBytesToInt(this byte[] value)
    {
        return BitConverter.ToInt32(value);
    }

    private static void ToBigEndian(byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
    }
}