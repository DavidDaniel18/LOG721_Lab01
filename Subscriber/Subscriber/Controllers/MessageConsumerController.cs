using SubscriberClient.Class;
using System.Diagnostics;
using Serilog;
using SmallTransit.Abstractions.Interfaces;

namespace SubscriberClient.Controllers
{
    public class MessageConsumerController : IConsumer<MessageLog721>
    {
        private readonly Metrics metrics;
        private readonly ILogger<MessageConsumerController> _logger;
        private DateTime? _lastMessageTime;

        public MessageConsumerController(Metrics metrics, ILogger<MessageConsumerController> logger)
        {
            this.metrics = metrics;
            _logger = logger;
        }

        public Task Consume(MessageLog721 contract)
        {
            var startTime = DateTime.UtcNow;

            try
            {
                metrics.CpuUsage = GetCpuUsage();
                metrics.MemoryUsageMB = GetMemoryUsageMB();
                var messageDateTime = contract.date_time;
                var messageId = contract.id;

                metrics.message = contract.message;

                var processingTime = DateTime.UtcNow - contract.date_time;
                metrics.ProcessingTime = processingTime;

                metrics.NumberOfMessagesSent++;

                if (_lastMessageTime.HasValue)
                {
                    var timeBetweenMessages = startTime - _lastMessageTime.Value;
                    metrics.AverageTimeBetweenMessages = CalculateAverageTime(metrics.AverageTimeBetweenMessages, timeBetweenMessages, metrics.NumberOfMessagesSent);
                }

                _lastMessageTime = startTime;

                _logger.LogInformation("Message Processing Time: {ProcessingTime} ms", processingTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement du message : {@Message}", contract);
            }

            return Task.CompletedTask;
        }

        private float GetCpuUsage()
        {
            using PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            cpuCounter.NextValue();

            Task.Delay(1000).Wait();

            return cpuCounter.NextValue();
        }

        private long GetMemoryUsageMB()
        {
            using PerformanceCounter memoryCounter = new PerformanceCounter("Memory", "Available Bytes");

            long availableMemoryBytes = (long)memoryCounter.NextValue();
            long availableMemoryMB = availableMemoryBytes / (1024 * 1024);

            return availableMemoryMB;
        }

        private TimeSpan CalculateAverageTime(TimeSpan currentAverage, TimeSpan newTime, int count)
        {
            return currentAverage + (newTime - currentAverage) / count;
        }
    }
}
