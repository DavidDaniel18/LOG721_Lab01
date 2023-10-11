using System.Diagnostics;
using Receiver.Entities;
using SmallTransit.Abstractions.Interfaces;

namespace Receiver.Controllers
{
    public class MessageConsumerController : IConsumer<WeatherForecast>
    {
        private readonly Metrics _metrics;
        private readonly ILogger<MessageConsumerController> _logger;
        private DateTime? _lastMessageTime;

        public MessageConsumerController(Metrics metrics, ILogger<MessageConsumerController> logger)
        {
            this._metrics = metrics;
            _logger = logger;
        }

        public Task Consume(WeatherForecast contract)
        {
            //var startTime = DateTime.UtcNow;

            //try
            //{
            //    _metrics.CpuUsage = GetCpuUsage();
            //    _metrics.MemoryUsageMB = GetMemoryUsageMB();

            //    var messageDateTime = contract.date_time;
            //    var messageId = contract.id;

            //    _metrics.message = contract.message;

            //    _metrics.RoutingKey = Environment.GetEnvironmentVariable("RoutingKey") ?? "*";

            //    var processingTime = DateTime.UtcNow - contract.date_time;
            //    _metrics.ProcessingTime = processingTime;

            //    _metrics.NumberOfMessagesSent++;

            //    if (_lastMessageTime.HasValue)
            //    {
            //        var timeBetweenMessages = startTime - _lastMessageTime.Value;
            //        _metrics.AverageTimeBetweenMessages = CalculateAverageTime(_metrics.AverageTimeBetweenMessages, timeBetweenMessages, _metrics.NumberOfMessagesSent);
            //    }

            //    _lastMessageTime = startTime;

            //    _logger.LogInformation("Message Processing Time: {ProcessingTime} ms", processingTime);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Erreur lors du traitement du message : {@Message}", contract);
            //}

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
