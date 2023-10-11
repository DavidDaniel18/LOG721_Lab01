using System.Text;

namespace SmallTransit.Domain.Common;

internal sealed class LabMqHeader
{
    private const int RoutingKeyByteLength = 4;
    private const int ContractNameByteLength = 4;

    private static int FixedHeaderSize => RoutingKeyByteLength + ContractNameByteLength;

    internal static byte[] GetHeaderBytes<TContract>(TContract payload) where TContract : class
    {
        var header = new byte[FixedHeaderSize];

        byte[] bytes = Encoding.ASCII.GetBytes(str);


        BitConverter.GetBytes((typeof(TContract).Name).CopyTo(0, header, 0, 4);

        BitConverter.GetBytes(payload.Length).CopyTo(header, 4);

        byte[] message = new byte[HeaderSize + payload.Length];
                   header.CopyTo(message, 0);
                              payload.CopyTo(message, HeaderSize);
                                         return message;
                                                })
    }
}