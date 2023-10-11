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

            var requestRouting = new RestRequest("/Publisher", Method.Post);

            requestRouting.AddQueryParameter("nbr_message", 1);
            requestRouting.AddQueryParameter("routing_key", "test");

            requestRouting.AddJsonBody(new
            {
                // Il n'y a pas de clé message
                anything = "Test",
            });

            RestResponse response = clientNodeController.Execute(requestRouting);

            Assert.IsTrue(response.StatusCode != System.Net.HttpStatusCode.OK);
        }


        [TestMethod]
        public async void Post_JsonValidData_ReturnsOkResult()
        {

            var clientNodeController = new RestClient($"http://{hostname}:{port}");

            var requestRouting = new RestRequest("/Publisher", Method.Post);

            requestRouting.AddQueryParameter("nbr_message", 1);
            requestRouting.AddQueryParameter("routing_key", "weather/montreal/temperature");
            
            requestRouting.AddJsonBody(new
            {
                message = "Test",
            });

            RestResponse response = clientNodeController.Execute(requestRouting);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = response.Content;

                // TODO : Vérifier le contenu de la réponse
            }
        }

        [TestMethod]
        public async void Post_XmlValidData_ReturnsOkResult()
        {

            var clientNodeController = new RestClient($"http://{hostname}:{port}");

            var requestRouting = new RestRequest("/Publisher", Method.Post);

            requestRouting.AddQueryParameter("nbr_message", 1);
            requestRouting.AddQueryParameter("routing_key", "test");

            requestRouting.AddXmlBody(new
            {
                message = "Test",
            });

            RestResponse response = clientNodeController.Execute(requestRouting);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = response.Content;

                // TODO : Vérifier le contenu de la réponse
            }
        }


        [TestMethod]
        public async void Post_MultipleJsonValidData_ReturnsOkResult()
        {

            var clientNodeController = new RestClient($"http://{hostname}:{port}");

            var requestRouting = new RestRequest("/Publisher", Method.Post);

            requestRouting.AddQueryParameter("nbr_message", 20);
            requestRouting.AddQueryParameter("routing_key", "weather/montreal/temperature");

            requestRouting.AddJsonBody(new
            {
                message = "Test",
            });

            RestResponse response = clientNodeController.Execute(requestRouting);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = response.Content;

                // TODO : Vérifier le contenu de la réponse et que les 20 messages ont été reçu
            }
        }


    }
}