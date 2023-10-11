using Interfaces.Domain;

namespace Interfaces.Services
{
    public interface IBrokerService
    {
        void AssignBroker(ISubscription subscription);
    }
}
