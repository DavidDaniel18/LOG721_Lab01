using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Application
{
    public class Router : IRouter
    {


        public ITopicNode root => ConcurrentTopicNode.Create();

        public void InitializeTopicNodes(List<string> allRoutePatterns)
        {
            throw new NotImplementedException();
        }
    }
}
