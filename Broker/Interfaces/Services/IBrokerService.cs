using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Domain;

namespace Interfaces.Services
{
    public interface IBrokerService
    {
        void AssignBroker(ISubscription subscription);
    }
}
