using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IRouter
    {
        ITopicNode root { get; }
        void InitializeTopicNodes(List<string> allRoutePatterns);
    }
}
