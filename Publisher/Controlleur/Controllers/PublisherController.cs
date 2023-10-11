using Controlleur.Classe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmallTransit.Abstractions.Interfaces;

namespace Configuration.Controllers
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
        public async Task<ActionResult> Post([FromBody] MessageLog721 message, int nbr_message, string routing_key)
        {
            for (int i = 0; i < nbr_message; i++)
            {
                var result = await _publisher.Publish(message, routing_key);

                if (result.IsFailure())
                {
                    return BadRequest("Une erreur est survenu. Votre message n'a pas été posté. Erreur: " + result.ToString());

                }
            }

            return Ok("Votre message a été publié à " + routing_key);
        }

        [HttpPost]
        [ActionName("SendTestMessage")]
        public async Task<ActionResult> SendTestMessage()
        {
            var result = await _publisher.Publish(new MessageLog721("90%"), "humidity/montreal");
            if (result.IsFailure())
            {
                return BadRequest("Une erreur est survenu. Votre message n'a pas été posté. Erreur: " + result.ToString());
            }

            result = await _publisher.Publish(new MessageLog721("20C"), "weather/montreal/temperature");
            if (result.IsFailure())
            {
                return BadRequest("Une erreur est survenu. Votre message n'a pas été posté. Erreur: " + result.ToString());
            }

            result = await _publisher.Publish(new MessageLog721("20C"), "weather/montreal/humidity");
            if (result.IsFailure())
            {
                return BadRequest("Une erreur est survenu. Votre message n'a pas été posté. Erreur: " + result.ToString());

            }

            return Ok("Vos messages de tests ont été publié" );
        }


    }
}