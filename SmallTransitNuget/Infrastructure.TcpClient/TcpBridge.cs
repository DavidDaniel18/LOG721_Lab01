using Infrastructure.TcpClient.L4LinkBuffers;

namespace Infrastructure.TcpClient;

internal sealed class TcpBridge
{
    private readonly ChannelTunnel _shore = new();

    private readonly StreamTunnel _tunnel;

    private const int TransferredBites = 1024;

    internal TcpBridge(Stream tunnel)
    {
        _tunnel = new StreamTunnel(tunnel);
    }

    internal async Task RunAsync(CancellationTokenSource cancellationTokenSource)
    {
        try
        {
            var buffer = new byte[TransferredBites];

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                var bytesRead = await _tunnel.ReadAsync(buffer, cancellationTokenSource.Token);
                
                if (bytesRead == 0) break;

                await _shore.WriteAsync(buffer[..bytesRead], cancellationTokenSource.Token);
            }
        }
        finally
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }
        }
    }

}