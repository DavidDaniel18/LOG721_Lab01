namespace SmallTransit.Domain.ProtoTransit.Extensions;

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
        if (value.Length > 4) throw new Exception("Value is too big to be converted to int");

        if (value.Length >1)
        {
            var index = value.Length-1;
            var zeros = 0;
            while (value[index].Equals(0))
            {
                index--;
                zeros++;
            }

            Array.Reverse(value, 0, value.Length-zeros);
        }

        byte[] paddedValue = new byte[4];

        // Copy the incoming big-endian bytes into the rightmost part of the array
        Array.Copy(value, 0, paddedValue, 0, value.Length);

        // Convert the byte array to an int
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