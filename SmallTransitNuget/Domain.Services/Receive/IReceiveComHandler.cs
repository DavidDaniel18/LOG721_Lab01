namespace Domain.Services.Receive;

internal interface IReceiveComHandler<in TContract>
{
    IAsyncEnumerable<byte[]> GetAccumulator();

    Task SendToController(TContract contract);
}