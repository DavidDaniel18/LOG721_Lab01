using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ISubscription
    {
        string Topic { get; set; }
        string Message { get; set; }
        Format format { get; set; }
        DateTime Timestamp { get; set; }
    }
}
