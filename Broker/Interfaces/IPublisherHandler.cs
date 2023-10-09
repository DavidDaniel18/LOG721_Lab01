using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPublisherHandler
    {
        bool Advertise(string topic, Format format);
        bool UnAdvertise(string topic, Format format);
        bool Publish(IPublication publication);
    }
}
