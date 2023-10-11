using Interfaces.Domain;

namespace Application
{
    public class ConcurrentTopicNode : ITopicNode
    {
        private ITopicNode _node = TopicNode.CreateRoot();
        private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        public string Key { 
            get => _node.Key;
            set { _node.Key = value; }
        }
        public ITopicNode? ParentTopicNode { get => _node.ParentTopicNode; set { _node.ParentTopicNode = value; } }
        public Dictionary<string, ITopicNode> ChildrenTopicNodes { get => _node.ChildrenTopicNodes; set { _node.ChildrenTopicNodes = value; } }

        public static ConcurrentTopicNode Create()
        {
            return new ConcurrentTopicNode();
        }
        private ConcurrentTopicNode() { }

        public void AddChildNode(ITopicNode node)
        {
            _lock.EnterWriteLock();
            _node.AddChildNode(node);
            _lock.ExitWriteLock();
        }

        public bool AddTopicNodes(string pattern)
        {
            _lock.EnterWriteLock();
            bool result = _node.AddTopicNodes(pattern);
            _lock.ExitWriteLock();
            return result;
        }

        public List<ITopicNode> GetAllChildren()
        {
            _lock.EnterReadLock();
            var result = _node.GetAllChildren();
            _lock.ExitReadLock();
            return result;
        }

        public List<ITopicNode> GetChildren()
        {
            _lock.EnterReadLock();
            var result = _node.GetChildren();
            _lock.ExitReadLock();
            return result;
        }

        public ITopicNode GetRoot()
        {
            _lock.EnterReadLock();
            var result = _node.GetRoot();
            _lock.ExitReadLock();
            return result;
        }

        public List<string> GetTopicLeafPaths()
        {
            _lock.EnterReadLock();
            var result = _node.GetTopicLeafPaths();
            _lock.ExitReadLock();
            return result;
        }

        public List<ITopicNode> GetTopicLeafs()
        {
            _lock.EnterReadLock();
            var result = _node.GetTopicLeafs();
            _lock.ExitReadLock();
            return result;
        }

        public Dictionary<string, ITopicNode> GetTopicNodes(string pattern)
        {
            _lock.EnterReadLock();
            var result = _node.GetTopicNodes(pattern);
            _lock.ExitReadLock();
            return result;
        }

        public string GetTopicPath()
        {
            _lock.EnterReadLock();
            var result = _node.GetTopicPath();
            _lock.ExitReadLock();
            return result;
        }

        public bool IsLeaf()
        {
            _lock.EnterReadLock();
            var result = _node.IsLeaf();
            _lock.ExitReadLock();
            return result;
        }

        public bool IsRoot()
        {
            _lock.EnterReadLock();
            var result = _node.IsRoot();
            _lock.ExitReadLock();
            return result;
        }

        public bool RemoveTopicNodes(string pattern)
        {
            _lock.EnterWriteLock();
            var result = _node.RemoveTopicNodes(pattern);
            _lock.ExitWriteLock();
            return result;
        }
    }
}
