namespace Receiver.Entities
{
    public class Metrics
    {
        public double ProcessingTimeMs { get; set; }
        public int NumberOfMessagesSent { get; set; }
        public double AverageTimeBetweenMessagesMs { get;  set; }
        public string Message { get; set; }
        public string RoutingKey { get; set; }
    }
}
