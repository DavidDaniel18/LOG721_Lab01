using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Domain;

namespace Interfaces.Services
{
    public interface IChannelService
    {
        void AddMessage(string route, IPublication publication);
        void AddChannel(string route);
        void RemoveChannel(string route);
    }
}
