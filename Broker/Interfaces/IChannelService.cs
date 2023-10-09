using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IChannelService
    {
        void AddMessage(string queueName, IPublication publication);
        void AddChannel(ISubscription subscription);
        void RemoveChannel(ISubscription subscription);
    }
}
