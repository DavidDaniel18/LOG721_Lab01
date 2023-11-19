using RestSharp;

namespace PublisherTest
{
    [TestClass]
    public class PublishControllerTests
    {
        private const string Host = "localhost";

        private static readonly Dictionary<Services, Address> Addresses = new()
        {
            { Services.Publisher, new Address(4304) },
            { Services.Subscriber, new Address(4300) },
            { Services.Subscriber2, new Address(4301) },
            { Services.Subscriber3, new Address(4302) },
            { Services.Subscriber4, new Address(4303) },
        };

        private RestClient _publisherClient = null!;
        private RestClient _subscriberClient = null!;
        private RestClient _subscriberClient2 = null!;
        private RestClient _subscriberClient3 = null!;
        private RestClient _subscriberClient4 = null!;


        [TestInitialize]
        public void InitClients()
        {
            _publisherClient = new RestClient(Addresses[Services.Publisher].GetValue());
            _subscriberClient = new RestClient(Addresses[Services.Subscriber].GetValue());
            _subscriberClient2 = new RestClient(Addresses[Services.Subscriber2].GetValue());
            _subscriberClient3 = new RestClient(Addresses[Services.Subscriber3].GetValue());
            _subscriberClient4 = new RestClient(Addresses[Services.Subscriber4].GetValue());
        }

        [TestMethod]
        public async Task Post_InValidData_ReturnsBadResult()
        {
            var requestRouting = new RestRequest("/Publisher/Post", Method.Post);

            requestRouting.AddQueryParameter("nbrMessage", 1);
            requestRouting.AddQueryParameter("routingKey", "humidity/montreal");

            requestRouting.AddJsonBody(new
            {
                // Il n'y a pas de cl� message
                anything = "Test",
            });

            var response = await _publisherClient.ExecutePostAsync(requestRouting);

            Assert.IsTrue(response.StatusCode != System.Net.HttpStatusCode.OK);
        }


        [TestMethod]
        public async Task Post_JsonValidData_ReturnsOkResult()
        {
            var requestRouting = new RestRequest("/Publisher/Post", Method.Post);

            requestRouting.AddQueryParameter("nbrMessage", 1);
            requestRouting.AddQueryParameter("routingKey", "weather/montreal/temperature");

            requestRouting.AddJsonBody(new MessageLog721("Test Json"));

            var response = await _publisherClient.ExecutePostAsync(requestRouting);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await Task.Delay(1000);

                var responseRequest = new RestRequest("Metrics");

                var metrics = await _subscriberClient.ExecuteGetAsync<Metrics>(responseRequest);
                
                Assert.IsTrue(response.IsSuccessful);

                Assert.IsTrue(metrics.Data.Message.Equals("Test Json"));
            }
        }

        private record Address(int Port, string Host = Host)
        {
            public string GetValue() => $"http://{Host}:{Port}";
        };

        enum Services
        {
            Publisher,
            Subscriber,
            Subscriber2,
            Subscriber3,
            Subscriber4
        }
    }
}