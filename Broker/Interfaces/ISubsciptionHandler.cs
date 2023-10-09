using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ISubsciptionHandler
    {
        void Subscribe(ISubscription subscription);
        void Unsubscribe(ISubscription subscription);
        void Litsen(ISubscription subscription);
    }
}
