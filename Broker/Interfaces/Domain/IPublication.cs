using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Domain
{
    public interface IPublication
    {
        string RoutingKey { get; }
        byte[] Message { get; }
    }
}
