using Interfaces.Domain;
using SmallTransit.Abstractions.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Publication : IPublication
    {
        public string Contract { get; }
        public string RoutingKey { get; }

        public byte[] Message { get; }

        public Publication(string contract, string routingKey, byte[] message)
        {
            Contract = contract;
            RoutingKey = routingKey;
            Message = message;
        }

        public static IPublication From(BrokerReceiveWrapper brokerReceiveWrapper)
        {
            return new Publication(brokerReceiveWrapper.Contract, brokerReceiveWrapper.RoutingKey, brokerReceiveWrapper.Payload);
        }
    }
}
