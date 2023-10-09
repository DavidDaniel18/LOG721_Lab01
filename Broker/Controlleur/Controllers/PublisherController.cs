using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Controllers.Controllers
{
    public class PublisherController
    {
        IPublisherHandler _publisherHandler;

        public PublisherController(IPublisherHandler publisherHandler)
        {
            _publisherHandler = publisherHandler;
        }

        public void OnAdvertise()
        {

        }

        public void OnUnAvertise()
        {

        }

        public void OnPublish(IPublication publication)
        {
            _publisherHandler.Publish(publication);
        }
    }
}
