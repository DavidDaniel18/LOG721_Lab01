using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IRouter
    {
        void InitializeTopicNodes(List<string> allRoutePatterns);
        void AddTopic(string pattern);
        void RemoveTopic(string pattern);
        IEnumerable<string> GetTopics(string pattern);
    }
}
