using Interfaces.Domain;

namespace Entities.Tests
{
    public class TopicNodeTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetTopicNodes_Test() 
        {
            ITopicNode root = ConcurrentTopicNode.Create();
            root.AddTopicNodes("this/is/a/test1");
            root.AddTopicNodes("this/is/a/test2");
            root.AddTopicNodes("this/is/a/test3");
            root.AddTopicNodes("this/is/a/test4");
            root.AddTopicNodes("this/is/a/test5/x");
            root.AddTopicNodes("this/is/a/test6/x");

            Dictionary<string, ITopicNode> nodes;

            nodes = root.GetTopicNodes("this/is/a/test1");
            Assert.AreEqual("this/is/a/test1", nodes["this/is/a/test1"].GetTopicPath());

            nodes = root.GetTopicNodes("this/is/a/*");
            Assert.AreEqual(6, nodes.Count());

            nodes = root.GetTopicNodes("this/is/a/*/x");
            Assert.AreEqual(2, nodes.Count());

            nodes = root.GetTopicNodes("this/is/a/#");
            Assert.AreEqual(9, nodes.Count());

            bool removed = root.RemoveTopicNodes("this/is/a/test6/x");
            Assert.AreEqual(true, removed);

            nodes = root.GetTopicNodes("this/is/a/*/x");
            Assert.AreEqual(1, nodes.Count());

            removed = root.RemoveTopicNodes("this/is");
            Assert.IsFalse(removed);

            List<string> leafs = root.GetTopicLeafPaths();

            nodes = root.GetTopicNodes("this/#");
            Assert.IsFalse(false);

            root.RemoveTopicNodes("this/is/a/test1");
            root.RemoveTopicNodes("this/is/a/test2");
            root.RemoveTopicNodes("this/is/a/test3");
            root.RemoveTopicNodes("this/is/a/test4");
            root.RemoveTopicNodes("this/is/a/test5/x");
            root.RemoveTopicNodes("this/is/a/test6/x");

            List<ITopicNode> n = root.GetAllChildren();
            Assert.AreEqual(0, n.Count());
        }
    }
}