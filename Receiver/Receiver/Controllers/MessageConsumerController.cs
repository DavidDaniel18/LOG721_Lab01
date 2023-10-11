using Receiver.Entities;
using SmallTransit.Abstractions.Interfaces;

namespace Receiver.Controllers
{
    public class MessageConsumerController : IConsumer<MessageLog721>
    {
        private readonly Metrics _metrics;
        private readonly ILogger<MessageConsumerController> _logger;
        private DateTime? _lastMessageTime;

        public MessageConsumerController(Metrics metrics, ILogger<MessageConsumerController> logger)
        {
            this._metrics = metrics;
            _logger = logger;
        }

        public Task Consume(MessageLog721 contract)
        {
            var startTime = DateTime.UtcNow;

            try
            {
                var messageDateTime = contract.date_time;
                var messageId = contract.id;

                _metrics.message = contract.message;

                _metrics.RoutingKey = Environment.GetEnvironmentVariable("RoutingKey") ?? "*";

                var processingTime = DateTime.UtcNow - contract.date_time;
                _metrics.ProcessingTime = processingTime;

                _metrics.NumberOfMessagesSent++;

                if (_lastMessageTime.HasValue)
                {
                    var timeBetweenMessages = startTime - _lastMessageTime.Value;
                    _metrics.AverageTimeBetweenMessages = CalculateAverageTime(_metrics.AverageTimeBetweenMessages, timeBetweenMessages, _metrics.NumberOfMessagesSent);
                }

                _lastMessageTime = startTime;

                _logger.LogInformation($"Message Processing Time: {processingTime.TotalMilliseconds} ms, btw the message is {contract.message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement du message : {@Message}", contract);
            }

            return Task.CompletedTask;
        }

        private TimeSpan CalculateAverageTime(TimeSpan currentAverage, TimeSpan newTime, int count)
        {
            return currentAverage + (newTime - currentAverage) / count;
        }
    }
}
