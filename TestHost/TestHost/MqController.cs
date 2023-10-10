using Application.Common.Broker;
using Application.Common.Interfaces;
using SmallTransit.Abstractions.Broker;

namespace TestHost;

internal sealed class MqController : IConsumer<BrokerSubscriptionDto>
{
    public async Task Consume(BrokerSubscriptionDto contract)
    {
        await contract.BrokerPushEndpoint.Push(new byte[100]);
    }
}

internal sealed class publishController : IConsumer<BrokerReceiveWrapper>
{
    public Task Consume(BrokerReceiveWrapper contract)
    {
        throw new NotImplementedException();
    }
}