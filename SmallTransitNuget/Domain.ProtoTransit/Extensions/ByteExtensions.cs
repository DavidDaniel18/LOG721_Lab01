namespace Domain.ProtoTransit.Extensions;

internal static class ByteExtensions
{
    internal static byte[] ToBytes(this int value)
    {
        var fullBytes = BitConverter.GetBytes(value);

        ToBigEndian(fullBytes);

        int startIndex = 0;

        // Find the first non-zero byte
        while (startIndex < fullBytes.Length && fullBytes[startIndex] == 0)
        {
            startIndex++;
        }

        // If all bytes are zero (i.e., the value is 0), return a single byte array
        if (startIndex == fullBytes.Length)
        {
            return new byte[] { 0 };
        }

        int length = fullBytes.Length - startIndex;

        byte[] result = new byte[length];

        Array.Copy(fullBytes, startIndex, result, 0, length);

        return result;
    }

    internal static int FromBytesToInt(this byte[] value)
    {
        byte[] paddedValue = new byte[4];

        Array.Copy(value, 0, paddedValue, 0, value.Length);

        return BitConverter.ToInt32(paddedValue, 0);
    }

    private static void ToBigEndian(byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
    }
}