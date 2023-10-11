using Application.Services.InfrastructureInterfaces;
using Domain.Common;
using Infrastructure.TcpClient.L4LinkBuffers;

namespace Infrastructure.TcpClient;

public sealed class TcpBridge : ITcpBridge
{
    private readonly ChannelTunnel _readShore = new();

    private readonly ChannelTunnel _writeShore = new();

    private const int TransferredBites = 1024;

    private Task _readTask = Task.CompletedTask;

    private Task _writeTask = Task.CompletedTask;

    public void RunAsync(Stream inputStream, Stream outputStream, CancellationTokenSource cancellationTokenSource)
    {
        var writeTunnel = new StreamTunnel(outputStream);
        var readTunnel = new StreamTunnel(inputStream);

        _readTask = Task.Run(() => ReadAsync(readTunnel, cancellationTokenSource));
        _writeTask = Task.Run(() => WriteAsync(writeTunnel, cancellationTokenSource));
    }

    private async Task ReadAsync(ITunnel tunnel, CancellationTokenSource cancellationTokenSource)
    {
        try
        {
            var buffer = new byte[TransferredBites];

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                var bytesRead = await tunnel.ReadAsync(buffer, cancellationTokenSource.Token);

                if (bytesRead > 0)
                {
                    await _readShore.WriteAsync(buffer[..bytesRead], cancellationTokenSource.Token);
                }
            }
        }
        finally
        {
            if (cancellationTokenSource.Token.IsCancellationRequested is false)
            {
                cancellationTokenSource.Cancel();
            }
        }
    }

    private async Task WriteAsync(ITunnel tunnel, CancellationTokenSource cancellationTokenSource)
    {
        try
        {
            await foreach (var bytes in _writeShore.GetAccumulator(cancellationTokenSource.Token))
            {
                await tunnel.WriteAsync(bytes, cancellationTokenSource.Token);
            }
        }
        finally
        {
            if (cancellationTokenSource.Token.IsCancellationRequested is false)
            {
                cancellationTokenSource.Cancel();
            }
        }
    }

    public async Task<Result> WaitForResponse(Func<byte[], Result<byte[]>> reminderSelector)
    {
        return await _readShore.WaitForResponse(reminderSelector);
    }

    public IAsyncEnumerable<byte[]> GetAccumulator()
    {
        return _readShore.GetAccumulator(CancellationToken.None);
    }

    public async Task<Result> SendMessage(byte[] value)
    {
        return await _writeShore.SendMessage(value);
    }
}