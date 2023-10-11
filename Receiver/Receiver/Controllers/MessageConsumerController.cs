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

            var processingTime = DateTime.UtcNow - contract.DateTime;

            _metrics.Message = contract.Message;

            _metrics.RoutingKey = Environment.GetEnvironmentVariable("RoutingKey") ?? "*";

            _metrics.ProcessingTimeMs = processingTime.TotalMilliseconds;

            _metrics.NumberOfMessagesSent++;

            if (_lastMessageTime.HasValue)
            {
                var timeBetweenMessages = startTime - _lastMessageTime.Value;

                _metrics.AverageTimeBetweenMessagesMs = CalculateAverageTime(_metrics.AverageTimeBetweenMessagesMs, timeBetweenMessages, _metrics.NumberOfMessagesSent);
            }

            _lastMessageTime = startTime;

            _logger.LogInformation($"Message Processing Time: {processingTime.TotalMilliseconds} ms, btw the message is {contract.Message}");

            return Task.CompletedTask;
        }

        private double CalculateAverageTime(double currentAverage, TimeSpan newTime, int count)
        {
            return (currentAverage + (newTime.TotalMilliseconds - currentAverage) / count);
        }
    }
}
