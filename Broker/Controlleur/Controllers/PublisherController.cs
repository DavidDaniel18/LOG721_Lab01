using Application.Common.Broker;
using Application.Common.Interfaces;
using SmallTransit.Abstractions.Broker;

internal sealed class BrokerControllerPublisher : IConsumer<BrokerReceiveWrapper>
{
    public Task Consume(BrokerReceiveWrapper contract)
    {
        throw new NotImplementedException();
    }
}