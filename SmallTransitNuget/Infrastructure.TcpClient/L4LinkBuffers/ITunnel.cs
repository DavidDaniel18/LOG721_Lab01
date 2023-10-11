namespace Infrastructure.TcpClient.L4LinkBuffers;

internal interface ITunnel : IDisposable
{
    internal ValueTask<int> ReadAsync(byte[] buffer, CancellationToken cancellationToken);

    internal ValueTask WriteAsync(byte[] buffer, CancellationToken cancellationToken);
}