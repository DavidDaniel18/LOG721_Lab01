using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text;

namespace Configuration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Subscribe : ControllerBase
    {
        private readonly ILogger<Subscribe> _logger;

        public Subscribe(ILogger<Subscribe> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public async Task GetWebSocket() 
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                //using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                //await Echo(webSocket);

                using var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
                while (true)
                {
                    var message = "The current time is: " + DateTime.Now.ToString("HH:mm:ss");
                    var bytes = Encoding.UTF8.GetBytes(message);
                    var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                    if (ws.State == WebSocketState.Open)
                        await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                    else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}