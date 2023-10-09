namespace PublisherTest
{
    [TestClass]
    public class PublishControllerTests
    {
        [TestMethod]
        public void Post_ValidData_ReturnsOkResult()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:32769/Publish";

                var data = new
                {
                    message = "mon message",
                };

                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();

                // Verifier que http code = 200
                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP POST request failed.");

                string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Assert.IsNotNull(responseContent);
            }
        }
    }
}