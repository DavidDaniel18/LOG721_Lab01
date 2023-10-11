using Interfaces.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IQueueService
    {
        string GetQueue(string routingKey);
        List<string> GetQueues(string routingKey);
        void AddPublication(string queueName, IPublication publication);
        void AddPublication(Channel<IPublication> queue, IPublication publication);
     }
}
