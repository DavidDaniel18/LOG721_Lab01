using Interfaces.Domain;

namespace Interfaces.Services
{
    public interface ISubscriptionService
    {
        void AddSubscription(ISubscription subscription);
        void RemoveSubscription(ISubscription subscription);
    }
}
