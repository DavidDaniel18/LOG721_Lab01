using Interfaces.Domain;

namespace Interfaces
{
    public interface IEndpoint
    {
        void Publish(IPublication publication);
    }
}
