using SmallTransit.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Domain
{
    public interface ISubscription
    {
        Guid Id { get; }
        string RoutingKey { get; }
        string QueueName { get; }
        string Type { get; }
        IBrokerPushEndpoint Endpoint { get; }
    }
}
