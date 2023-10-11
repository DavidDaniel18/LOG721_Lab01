using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Domain;

namespace Interfaces.Handler
{
    public interface IPublisherHandler
    {
        void Advertise(string route);
        void UnAdvertise(string route);
        void Publish(IPublication publication);
    }
}
