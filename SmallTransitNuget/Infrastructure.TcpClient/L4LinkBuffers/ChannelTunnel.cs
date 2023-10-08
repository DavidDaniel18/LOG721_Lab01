using System.Threading.Channels;

namespace Infrastructure.TcpClient.L4LinkBuffers;

internal sealed class ChannelTunnel : IByteAccumulator
{
    private const int TransferredBites = 1024;

    private readonly Channel<byte[]> _channel = Channel.CreateUnbounded<byte[]>(new UnboundedChannelOptions
    {
        SingleReader = true,
        SingleWriter = true,
    });

    private MemoryStream _memoryStream = new();

    public void Dispose()
    {
        _channel.Writer.TryComplete();
        _memoryStream.Dispose();
    }

    public IAsyncEnumerable<byte[]> GetAccumulator(CancellationToken cancellationToken) => _channel.Reader.ReadAllAsync(cancellationToken);

    //public async Task<byte[]> ReadAsync(CancellationToken cancellationToken)
    //{
    //    var buffer = new byte[TransferredBites];

    //    if (_memoryStream.Length == _memoryStream.Position) // If we have read everything from the MemoryStream
    //    {
    //        if (!await RefillBufferAsync(cancellationToken)) // If we can't read more from the Channel
    //        {
    //            return default; // Signal the end of the stream
    //        }
    //    }

    //    // Read from the MemoryStream
    //    _ = await _memoryStream.ReadAsync(buffer, cancellationToken);

    //    return buffer;
    //}

    //private async ValueTask<bool> RefillBufferAsync(CancellationToken cancellationToken)
    //{
    //    byte[] bytesFromChannel;
    //    try
    //    {
    //        if (!_channel.Reader.TryRead(out bytesFromChannel))
    //        {
    //            bytesFromChannel = await _channel.Reader.ReadAsync(cancellationToken);
    //        }
    //    }
    //    catch (ChannelClosedException)
    //    {
    //        return false; // Signal the end of the stream
    //    }

    //    _memoryStream = new MemoryStream(bytesFromChannel);

    //    return true;
    //}

    public async Task WriteAsync(byte[] buffer, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(buffer, cancellationToken);
    }
}