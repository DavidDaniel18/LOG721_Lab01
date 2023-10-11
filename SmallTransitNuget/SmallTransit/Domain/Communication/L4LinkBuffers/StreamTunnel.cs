namespace SmallTransit.Domain.Communication.L4LinkBuffers;

/// <summary>
///   A tunnel is a wrapper around a stream that allows for reading and writing to an external TCP connection.
/// </summary>
internal sealed class StreamTunnel : ITunnel
{
    private readonly Stream _stream;

    internal StreamTunnel(Stream stream)
    {
        _stream = stream;
    }

    public void Dispose()
    {
        _stream.Dispose();
    }

    public ValueTask<int> ReadAsync(byte[] bufferArray, CancellationToken cancellationToken)
    {
        return _stream.ReadAsync(bufferArray, cancellationToken);
    }

    public ValueTask WriteAsync(byte[] buffer, CancellationToken cancellationToken)
    {
        return _stream.WriteAsync(buffer, cancellationToken);
    }
}