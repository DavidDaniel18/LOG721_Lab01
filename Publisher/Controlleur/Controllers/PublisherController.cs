using Controlleur.Classe;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Interfaces;

namespace Controlleur.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PublisherController : ControllerBase
    {
        private readonly ILogger<PublisherController> _logger;
        private readonly IPublisher<MessageLog721> _publisher;

        public PublisherController(ILogger<PublisherController> logger, IPublisher<MessageLog721> publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        [HttpPost]
        [ActionName("Post")]
        public async Task<ActionResult> Post([FromBody] MessageLog721 message, int nbrMessage, string routingKey)
        {
            for (int i = 0; i < nbrMessage; i++)
            {
                var result = await _publisher.Publish(message, routingKey);

                if (result.IsFailure())
                {
                    return BadRequest("Une erreur est survenu. Votre message n'a pas été posté. Erreur: " + result.Exception);
                }
            }

            return Ok("Votre message a été publié à " + routingKey);
        }

        [HttpPost]
        [ActionName("Message")]
        public async Task<ActionResult> Message(string message, int nbrMessage, string routingKey)
        {
            for (int i = 0; i < nbrMessage; i++)
            {
                var result = await _publisher.Publish(new MessageLog721(message), routingKey);

                if (result.IsFailure())
                {
                    return BadRequest("Une erreur est survenu. Votre message n'a pas été posté. Erreur: " + result.Exception);
                }
            }

            return Ok("Votre message a été publié à " + routingKey);
        }

        [HttpPost]
        [ActionName("SendTestMessage")]
        public async Task<ActionResult> SendTestMessage()
        {
            var result = await _publisher.Publish(new MessageLog721("90%"), "humidity/montreal")
                .BindAsync(async () => await _publisher.Publish(new MessageLog721("85%"), "humidity"))
                .BindAsync(async () => await _publisher.Publish(new MessageLog721("35C"), "weather/montreal/temperature"))
                .BindAsync(async () => await _publisher.Publish(new MessageLog721("-10C"), "weather/montreal/humidity"));

            if (result.IsFailure()) return BadRequest($"Une erreur est survenu. {result.Exception!.Message}");

            return Ok("Vos messages de tests ont été publié" );
        }
    }
}