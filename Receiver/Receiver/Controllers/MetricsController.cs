using Microsoft.AspNetCore.Mvc;
using Receiver.Entities;

namespace Receiver.Controllers
{
    [Route("api/metrics")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
        private readonly Metrics metrics;
        private readonly ILogger<MetricsController> _logger;
        public MetricsController(Metrics metrics, ILogger<MetricsController> logger)
        {
            this.metrics = metrics;
            _logger = logger;

        }
        [HttpGet("gatherMetrics")]
        public ActionResult<Metrics> GetMetrics() {
            
            return Ok(metrics);
        }
    
    }
}
