namespace PublisherTest
{
    public class Metrics
    {
        public TimeSpan ProcessingTime { get; set; }
        public int NumberOfMessagesSent { get; set; }
        public TimeSpan AverageTimeBetweenMessages { get; set; }
        public string Message { get; set; }
        public string RoutingKey { get; set; }
    }
}