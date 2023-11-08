using NUnit.Framework;


namespace Data
{
    [TestFixture]
    public class DataTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetDistance_Test()
        {
            Data data1 = new Data(50, 100);
            Data data2 = new Data(150, 200);

            double result = data1.GetDistance(data2);
            double expected_result = 100 * Math.Sqrt(2);

            Assert.AreEqual(expected_result, result);
        }
    }
}