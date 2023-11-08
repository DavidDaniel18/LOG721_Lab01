using NUnit.Framework;


namespace Data
{
    [TestFixture]
    public class MapTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void createMaps_Test()
        {
            List<Group> groups = new List<Group>
            {
                new Group(1, new Data(50, 100)),
                new Group(2, new Data(100, 150)),
                new Group(3, new Data(150, 200)),
                new Group(4, new Data(200, 250)),
                new Group(5, new Data(250, 300))
            };
            List<Data> datas = new List<Data>
            {
                new Data(104, 162),
            };

            Dictionary<Group, List<Data>> expected_results = new Dictionary<Group, List<Data>>
            {
                { 
                    new Group(1, new Data(50, 100)), 
                    new List<Data>()
                },
                { 
                    new Group(2, new Data(100, 150)),
                    new List<Data>
                    {
                        new Data(104, 162),
                    }
                },
                { 
                    new Group(3, new Data(150, 200)),
                    new List<Data>()

                },
                { 
                    new Group(4, new Data(200, 250)),
                    new List<Data>()
                },
                { 
                    new Group(5, new Data(250, 300)),
                    new List<Data>()
                },
            };


            Dictionary<Group, List<Data>> results = Map.createMaps(groups, datas);
            CollectionAssert.AreEqual(expected_results, results);
        }
    }
}