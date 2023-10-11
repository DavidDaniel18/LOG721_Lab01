namespace Interfaces.Domain
{
    public interface IPublication
    {
        string Contract { get; }
        string RoutingKey { get; }
        byte[] Message { get; }
    }
}
