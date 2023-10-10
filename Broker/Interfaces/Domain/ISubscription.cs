using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Domain
{
    public interface ISubscription
    {
        Guid Id { get; set; }
        string RoutingKey { get; set; }
        string Type { get; set; }
        IEndpoint Endpoint { get; set; }
    }
}
