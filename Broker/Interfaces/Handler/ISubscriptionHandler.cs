using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Domain;

namespace Interfaces.Handler
{
    public interface ISubscriptionHandler
    {
        void Subscribe(ISubscription subscription);
        void Unsubscribe(ISubscription subscription);
        void Listen(ISubscription subscription);
    }
}
