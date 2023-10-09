using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ISubscriptionService
    {
        void AddSubscription(ISubscription subscription);
        void RemoveSubscription(ISubscription subscription);
    }
}
