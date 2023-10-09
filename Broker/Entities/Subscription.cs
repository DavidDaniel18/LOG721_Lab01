using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Subscription : ISubscription
    {
        public required string Topic { get; set; }
        public required string Message { get; set; }
        public Format format { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
