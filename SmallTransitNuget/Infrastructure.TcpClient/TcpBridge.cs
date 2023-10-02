using Infrastructure.TcpClient.L4LinkBuffers;

namespace Infrastructure.TcpClient;

internal sealed class TcpBridge
{
    private readonly ChannelTunnel _middleware = new();

    private readonly StreamTunnel _sourceTunnel;
    private readonly StreamTunnel _targetTunnel;

    private const int TransferredBites = 1024;

    internal TcpBridge(Stream source, Stream target)
    {
        _sourceTunnel = new StreamTunnel(source);
        _targetTunnel = new StreamTunnel(target);
    }

    internal async Task RunAsync(CancellationTokenSource cancellationTokenSource)
    {
        var sourceToMiddlewareTask = Task.Run(async () =>
        {
            var buffer = new byte[TransferredBites];
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                var bytesRead = await _sourceTunnel.ReadAsync(buffer, cancellationTokenSource.Token);
                if (bytesRead == 0)
                {
                    break;
                }
                await _middleware.WriteAsync(buffer[..bytesRead], cancellationTokenSource.Token);
            }
        }, cancellationTokenSource.Token);

        var middlewareToTargetTask = Task.Run(async () =>
        {
            var buffer = new byte[TransferredBites];
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                var bytesRead = await _middleware.ReadAsync(buffer, cancellationTokenSource.Token);
                if (bytesRead == 0)
                {
                    break;
                }
                await _targetTunnel.WriteAsync(buffer[..bytesRead], cancellationTokenSource.Token);
            }
        }, cancellationTokenSource.Token);

        try
        {
            await Task.WhenAny(sourceToMiddlewareTask, middlewareToTargetTask);
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