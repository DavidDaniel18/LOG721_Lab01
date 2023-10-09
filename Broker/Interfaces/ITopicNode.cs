using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Interfaces
{
    public interface ITopicNode
    {
        string Key { get; set; }
        ITopicNode? ParentTopicNode { get; set; }
        Dictionary<string, ITopicNode> ChildrenTopicNodes { get; set; }
        bool IsLeaf();
        ITopicNode GetRoot();
        List<string> GetTopicLeafPaths();
        List<ITopicNode> GetTopicLeafs();
        string GetTopicPath();
        List<ITopicNode> GetChildren();
        List<ITopicNode> GetAllChildren();
        Dictionary<string, ITopicNode> GetTopicNodes(string pattern);
        void AddChildNode(ITopicNode node);
        bool IsRoot();
        bool AddTopicNodes(string pattern);
        bool RemoveTopicNodes(string pattern);
    }
}