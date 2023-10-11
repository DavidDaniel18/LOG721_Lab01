namespace Receiver.Entities
{
    public class Metrics
    {
        public TimeSpan ProcessingTime { get; set; }    
        public float CpuUsage { get; set; }
        public long MemoryUsageMB { get; set; }
        public int NumberOfMessagesSent { get; set; }
        public TimeSpan AverageTimeBetweenMessages { get;  set; }
        public string message { get; set; }
        public string RoutingKey { get; set; }
    }
}
