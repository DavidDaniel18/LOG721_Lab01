namespace PublisherTest
{
    public class MessageLog721
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Message { get; set; }

        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        public MessageLog721(string message)
        {
            Message = message;
        }
    }
}