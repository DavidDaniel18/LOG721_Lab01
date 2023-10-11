using SmallTransit;
using SubscriberClient.Class;
using System.Diagnostics;
using Serilog;
using Newtonsoft.Json;
using System.Diagnostics;
using System;
using System.Drawing.Text;
using SmallTransit.Abstractions.Interfaces;

namespace SubscriberClient.Controllers
{
    public class MessageConsumerController : IConsumer<MessageLog721>
    {
        private readonly Metrics metrics;
        private DateTime? lastMessageTime;

        public async Task Consume(MessageLog721 message)
        {
            

            
            var startTime = DateTime.UtcNow;

           
            try
            {
                metrics.CpuUsage = GetCpuUsage();
                metrics.MemoryUsageMB = GetMemoryUsageMB();
                var messageDateTime = message.date_time;
                var messageId = message.id;

                metrics.message = message.message;


                var processingTime = DateTime.UtcNow - message.date_time;
                metrics.ProcessingTime = processingTime;

                metrics.NumberOfMessagesSent++;
                if (lastMessageTime.HasValue)
                {
                    var timeBetweenMessages = startTime - lastMessageTime.Value;
                    metrics.AverageTimeBetweenMessages = CalculateAverageTime(metrics.AverageTimeBetweenMessages, timeBetweenMessages, metrics.NumberOfMessagesSent);
                }

                lastMessageTime = startTime;
                Log.Information("Message Processing Time: {ProcessingTime} ms", processingTime);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors du traitement du message : {@Message}", message);
            }
        }


        private float GetCpuUsage()
        {
            using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                cpuCounter.NextValue();
                System.Threading.Thread.Sleep(1000);
                return cpuCounter.NextValue();
            }
        }

        private long GetMemoryUsageMB()
        {
            using (PerformanceCounter memoryCounter = new PerformanceCounter("Memory", "Available Bytes"))
            {
                long availableMemoryBytes = (long)memoryCounter.NextValue();
                long availableMemoryMB = availableMemoryBytes / (1024 * 1024);
                return availableMemoryMB;
            }
        }

        private TimeSpan CalculateAverageTime(TimeSpan currentAverage, TimeSpan newTime, int count)
        {
            return currentAverage + (newTime - currentAverage) / count;
        }

    }
}
