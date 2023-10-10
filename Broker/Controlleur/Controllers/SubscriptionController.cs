using Application.Common.Broker;
using Application.Common.Interfaces;
using SmallTransit.Abstractions.Broker;

internal sealed class BrokerControllerSubscription : IConsumer<BrokerSubscriptionDto>
{
    public async Task Consume(BrokerSubscriptionDto contract)
    {
        await contract.BrokerPushEndpoint.Push(new byte[100]);
    }
}