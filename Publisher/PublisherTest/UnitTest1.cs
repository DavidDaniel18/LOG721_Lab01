using Newtonsoft.Json;
using RestSharp;

namespace PublisherTest
{
    [TestClass]
    public class PublishControllerTests
    {
        string hostname = "localhost";
        string port = "32768";

        [TestMethod]
        public async void Post_InValidData_ReturnsBadResult()
        {

            var clientNodeController = new RestClient($"http://{hostname}:{port}");

            var requestRouting = new RestRequest("/Receiver1", Method.Post);

            requestRouting.AddQueryParameter("nbr_message", 1);
            requestRouting.AddQueryParameter("routing_key", "humidity/montreal");

            requestRouting.AddJsonBody(new
            {
                // Il n'y a pas de cl� message
                anything = "Test",
            });

            RestResponse response = clientNodeController.Execute(requestRouting);

            Assert.IsTrue(response.StatusCode != System.Net.HttpStatusCode.OK);
        }


        [TestMethod]
        public async void Post_JsonValidData_ReturnsOkResult()
        {

            var clientNodeController = new RestClient($"http://{hostname}:{port}");

            var requestRouting = new RestRequest("/Receiver2", Method.Post);

            requestRouting.AddQueryParameter("nbr_message", 1);
            requestRouting.AddQueryParameter("routing_key", "weather/montreal/temperature");

            requestRouting.AddJsonBody(new
            {
                message = "Test Json",
            });

            RestResponse response = clientNodeController.Execute(requestRouting);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseRequest = new RestRequest("/Subscriber1", Method.Get);

                response = clientNodeController.Execute(requestRouting);
                Assert.IsTrue(response.IsSuccessful);

                Metrics metrics = JsonConvert.DeserializeObject<Metrics>(response.Content);

                Assert.Equals(metrics.message, "Test Json");
            }
        }

        [TestMethod]
        public async void Post_XmlValidData_ReturnsOkResult()
        {

            var clientNodeController = new RestClient($"http://{hostname}:{port}");

            var requestRouting = new RestRequest("/Receiver3", Method.Post);

            requestRouting.AddQueryParameter("nbr_message", 1);
            requestRouting.AddQueryParameter("routing_key", "weather/montreal/humidity");

            requestRouting.AddXmlBody(new
            {
                message = "Test xml",
            });

            RestResponse response = clientNodeController.Execute(requestRouting);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseRequest = new RestRequest("/Subscriber2", Method.Get);

                response = clientNodeController.Execute(requestRouting);
                Assert.IsTrue(response.IsSuccessful);

                Metrics metrics = JsonConvert.DeserializeObject<Metrics>(response.Content);

                Assert.Equals(metrics.message, "Test xml");
            }
        }


        [TestMethod]
        public async void Post_MultipleJsonValidData_ReturnsOkResult()
        {

            var clientNodeController = new RestClient($"http://{hostname}:{port}");

            var requestRouting = new RestRequest("/Receiver2", Method.Post);

            requestRouting.AddQueryParameter("nbr_message", 20);
            requestRouting.AddQueryParameter("routing_key", "weather/montreal/temperature");

            requestRouting.AddJsonBody(new
            {
                message = "Test Json",
            });

            RestResponse response = clientNodeController.Execute(requestRouting);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseRequest = new RestRequest("/Subscriber2", Method.Get);

                response = clientNodeController.Execute(requestRouting);
                Assert.IsTrue(response.IsSuccessful);

                Metrics metrics = JsonConvert.DeserializeObject<Metrics>(response.Content);

                Assert.Equals(metrics.NumberOfMessagesSent, 20);
                Assert.Equals(metrics.message, "Test Json");
            }
        }

        [TestMethod]
        public async void Post_MultipleXmlValidData_ReturnsOkResult()
        {

            var clientNodeController = new RestClient($"http://{hostname}:{port}");

            var requestRouting = new RestRequest("/Receiver2", Method.Post);

            requestRouting.AddQueryParameter("nbr_message", 20);
            requestRouting.AddQueryParameter("routing_key", "weather/montreal/temperature");

            requestRouting.AddXmlBody(new
            {
                message = "Test Xml",
            });

            RestResponse response = clientNodeController.Execute(requestRouting);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseRequest = new RestRequest("/Subscriber2", Method.Get);

                response = clientNodeController.Execute(requestRouting);
                Assert.IsTrue(response.IsSuccessful);

                Metrics metrics = JsonConvert.DeserializeObject<Metrics>(response.Content);

                Assert.Equals(metrics.NumberOfMessagesSent, 20);
                Assert.Equals(metrics.message, "Test Xml");
            }
        }

    }
}