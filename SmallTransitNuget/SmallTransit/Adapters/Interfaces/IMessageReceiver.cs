namespace SmallTransit.Adapters.Interfaces;

public interface IBrokerMessageReceiver<TContract> where TContract : class
{
    public Task ReceiveAsync(TContract channel);
}