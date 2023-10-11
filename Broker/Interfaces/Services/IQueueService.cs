using Interfaces.Domain;
using System.Threading.Channels;

namespace Interfaces.Services
{
    public interface IQueueService
    {
        List<string> GetQueues(string routingKey);
        void AddPublication(string queueName, IPublication publication);
        void AddPublication(Channel<IPublication> queue, IPublication publication);
     }
}
