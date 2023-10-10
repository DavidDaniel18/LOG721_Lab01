
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Xml.Linq;
using Interfaces;
using Interfaces.Domain;

namespace Application
{
    public class TopicNode : ITopicNode
    {
        private const string AllSubsequentLayersPattern = "#";
        private const string SubsequentLayerPattern = "*";
        private const char Separator = '.';

        public string Key { get; set; } = "default_root_should_never_appear_in_path";
        public ITopicNode? ParentTopicNode { get; set; }
        public Dictionary<string, ITopicNode> ChildrenTopicNodes { get; set; } = new Dictionary<string, ITopicNode>();

        public List<IObserver> Observers => _observers;

        private List<IObserver> _observers = new List<IObserver>();

        public static ITopicNode CreateRoot()
        {
            return new TopicNode();
        }

        private TopicNode() { }

        public TopicNode(string key, ITopicNode? parentTopicNode)
        {
            Key = key;
            ParentTopicNode = parentTopicNode;
        }

        public void AddChildNode(ITopicNode node)
        {
            ChildrenTopicNodes.Add(node.Key, node);
        }

        public List<ITopicNode> GetChildren()
        {
            return ChildrenTopicNodes.Values.ToList();
        }

        public List<ITopicNode> GetAllChildren()
        {
            List<ITopicNode> nodes = new List<ITopicNode>();
            GetAllChildren(this, nodes);
            return nodes;
        }

        private void GetAllChildren(ITopicNode node, List<ITopicNode> nodes)
        {
            if (node.IsLeaf())
                return;

            foreach (ITopicNode child in node.GetChildren())
            {
                GetAllChildren(child, nodes);
                nodes.Add(child);
            }
        }

        public ITopicNode GetRoot()
        {
            return ParentTopicNode == null ? this : ParentTopicNode.GetRoot();
        }

        public string GetTopicPath()
        {
            List<string> topicKeys = new List<string>();
            ITopicNode? node = this;
            while (node != null && !node.IsRoot())
            {
                topicKeys.Insert(0, node.Key);
                node = node.ParentTopicNode;
            }
            return string.Join(Separator, topicKeys.ToArray());
        }

        public bool IsRoot()
        {
            return ParentTopicNode == null;
        }

        public bool IsLeaf()
        {
            return ChildrenTopicNodes.Count == 0;
        }

        public bool AddTopicNodes(string pattern)
        {
            return IsRoot() 
                ? AddTopicNodes(this, pattern.Split(Separator), 0) 
                : GetRoot().AddTopicNodes(pattern);
        }

        private bool AddTopicNodes(ITopicNode node, string[] patternKeys, int index)
        {
            var key = patternKeys[index];
            var nextNodeAdded = false;
            ITopicNode? nextNode;
            node.ChildrenTopicNodes.TryGetValue(key, out nextNode);

            if (nextNode == null)
            {
                nextNode = new TopicNode { Key = key, ParentTopicNode = node };
                node.AddChildNode(nextNode);
                nextNodeAdded = true;
            }

            if (index < patternKeys.Length - 1)
                return AddTopicNodes(nextNode, patternKeys, index + 1);

            return nextNodeAdded;
        }

        public Dictionary<string, ITopicNode> GetTopicNodes(string pattern)
        {
            Dictionary<string, ITopicNode> nodesMap = new Dictionary<string, ITopicNode>();
            GetTopicNodes(nodesMap, GetRoot(), pattern.Split(Separator), 0);
            return nodesMap;
        }

        private void GetTopicNodes(Dictionary<string, ITopicNode> nodesMap, ITopicNode node, string[] patternKeys, int index)
        {
            if (index == patternKeys.Length)
                return;

            bool isLastPatternKey = index == patternKeys.Length - 1;
            string key = patternKeys[index];

            if (string.Equals(AllSubsequentLayersPattern, key))
            {
                nodesMap.Add(node.GetTopicPath(), node);

                foreach (ITopicNode n in node.GetAllChildren())
                    nodesMap.Add(n.GetTopicPath(), n);
            }
            else if (string.Equals(SubsequentLayerPattern, key))
            {
                if (isLastPatternKey)
                    foreach (ITopicNode n in node.GetChildren())
                        nodesMap.Add(n.GetTopicPath(), n);
                else
                    foreach (ITopicNode n in node.GetChildren())
                        GetTopicNodes(nodesMap, n, patternKeys, index + 1);
            }
            else
            {
                ITopicNode? nextNode;
                node.ChildrenTopicNodes.TryGetValue(key, out nextNode);

                if (nextNode != null)
                    if (isLastPatternKey)
                        nodesMap.Add(nextNode.GetTopicPath(), nextNode);
                    else
                        GetTopicNodes(nodesMap, nextNode, patternKeys, index + 1);
            }
        }

        public bool RemoveTopicNodes(string pattern)
        {
            return RemoveTopicNodes(GetRoot(), pattern.Split(Separator), 0);
        }

        private bool RemoveTopicNodes(ITopicNode node, string[] patternKeys, int index)
        {
            var key = patternKeys[index];
            if (index == patternKeys.Length - 1)
            {
                ITopicNode? lastNode, parent;
                
                node.ChildrenTopicNodes.TryGetValue(key, out lastNode);
                
                if (!(lastNode?.IsLeaf() ?? false))
                    return false;

                parent = node;
                
                if (!(parent?.ChildrenTopicNodes.Remove(key) ?? false))
                    return false;

                while (parent != null)
                {
                    if (parent.ChildrenTopicNodes.Count() == 0)
                        parent.ParentTopicNode?.ChildrenTopicNodes.Remove(parent.Key);
                    
                    parent = parent.ParentTopicNode;
                }
                return true;
            }

            ITopicNode? nextNode;
            node.ChildrenTopicNodes.TryGetValue(key, out nextNode);

            if (nextNode != null)
                return RemoveTopicNodes(nextNode, patternKeys, index + 1);

            return false;
        }

        public List<string> GetTopicLeafPaths()
        {
            return GetTopicLeafs().Select(x => x.GetTopicPath()).ToList();
        }

        public List<ITopicNode> GetTopicLeafs()
        {
            return GetRoot().GetAllChildren().FindAll(x => x.IsLeaf());
        }
    }
}