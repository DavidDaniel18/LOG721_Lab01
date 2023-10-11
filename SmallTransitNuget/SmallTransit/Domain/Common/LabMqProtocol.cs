namespace SmallTransit.Domain.Common;

internal sealed class LabMqProtocol
{
    // Header is 4 bytes message type + 4 bytes payload length

    public static byte[] CreateMessage<TContract>(TContract payload) where TContract : class
    {
        var header = new byte[FixedHeaderSize];



        BitConverter.GetBytes((typeof(TContract).Name).CopyTo(header, 0);
        BitConverter.GetBytes(payload.Length).CopyTo(header, 4);

        byte[] message = new byte[HeaderSize + payload.Length];
        header.CopyTo(message, 0);
        payload.CopyTo(message, HeaderSize);

        return message;
    }

    public static void ParseMessage(byte[] message, out MessageType type, out byte[] payload)
    {
        type = (MessageType)BitConverter.ToInt32(message, 0);
        int payloadSize = BitConverter.ToInt32(message, 4);

        payload = new byte[payloadSize];
        Array.Copy(message, HeaderSize, payload, 0, payloadSize);
    }
}