using Controlleur.Classe;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Xml;
using RabbitMQ.Client;
using MassTransit.Transports.Fabric;
using MassTransit.Internals.GraphValidation;
using System;
using Controlleur.Interface;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;

namespace Configuration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Publish : ControllerBase
    {
        private readonly ILogger<Publish> _logger;

        public Publish(ILogger<Publish> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Message message, int nbr_message, string routing_key)
        {
            //var contentType = Request.ContentType;


            /*using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string body = reader.ReadToEndAsync().Result;
                Message message;

                if (contentType == "application/json")
                {
                    message = Publication.fromJSONtoCanonical(JsonDocument.Parse(body));
                }
                else if (contentType == "application/json")
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(body);

                    message = Publication.fromXMLtoCanonical(xmlDoc);
                }
                else
                {
                    return BadRequest("Le format \"" + contentType + "\" n'est pas supporté.");
                }
            }*/

            // comment obtenir le publisher?
            publisher.Publish(JsonConvert.SerializeObject(object), routing_key, headers);



            return Ok();
        }

    }
}