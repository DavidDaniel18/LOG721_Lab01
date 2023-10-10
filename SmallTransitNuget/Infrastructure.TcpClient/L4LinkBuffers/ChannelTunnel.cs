using System.Threading.Channels;
using Domain.Common;

namespace Infrastructure.TcpClient.L4LinkBuffers;

internal sealed class ChannelTunnel
{
    private const int TransferredBites = 1024;

    byte[] bytesStorage = Array.Empty<byte>();

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

    public async Task WriteAsync(byte[] buffer, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(buffer, cancellationToken);
    }

    public async Task<Result> SendMessage(byte[] value)
    {
        try
        {
            await WriteAsync(value, CancellationToken.None);

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }
    }

    public async Task<Result> WaitForResponse(Func<byte[], Result<byte[]>> reminderSelector)
    {
        await foreach (var bytes in GetAccumulator(CancellationToken.None))
        {
            bytesStorage = bytesStorage.Concat(bytes).ToArray();

            var reminderResult = reminderSelector(bytesStorage);

            if (reminderResult.IsSuccess())
            {
                bytesStorage = reminderResult.Content!;
            }

            return reminderResult;
        }

        return Result.Failure("Channel was closed");
    }
}