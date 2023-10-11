using Domain.Services.Receive.BrokerReceive;
using Domain.Services.Receive.ClientReceive;
using Domain.Services.Send.Publishing;
using Domain.Services.Send.Push;
using Domain.Services.Send.Subscribing;

namespace Infrastructure.Cache;

internal sealed class TransitCache
{
    private static readonly HashSet<PublishContext> _publishContexts = new();
    private static readonly HashSet<PushContext> _pushContexts = new();
    private static readonly HashSet<SubscribeContext> _subscribeContexts = new();
    private static readonly HashSet<ClientReceiveContext> _clientReceiveContexts = new();
    private static readonly HashSet<BrokerReceiveContext> _brokerReceiveContexts = new();
}