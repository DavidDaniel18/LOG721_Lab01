using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IQueueService
    {
        bool AddQueue(string route, Format format);
        bool RemoveQueue(string route, Format format);
        bool Publish(IPublication publication);
    }
}
