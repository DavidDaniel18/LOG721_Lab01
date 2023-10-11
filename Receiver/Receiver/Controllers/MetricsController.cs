using Microsoft.AspNetCore.Mvc;
using Receiver.Entities;

namespace Receiver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly Metrics _metrics;

        public MetricsController(Metrics metrics)
        {
            this._metrics = metrics;

        }

        [HttpGet]
        public ActionResult<Metrics> GetMetrics() {
            
            return Ok(_metrics);
        }
    
    }
}
