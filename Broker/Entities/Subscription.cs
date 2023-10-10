using Interfaces;
using Interfaces.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Subscription : ISubscription
    {
        public IEndpoint? Endpoint { get; set; }
        public Guid Id { get; set; }
        public string RoutingKey { get; set; }
        public string Type { get; set; }

        public Subscription()
        {
            
        }
    }
}
