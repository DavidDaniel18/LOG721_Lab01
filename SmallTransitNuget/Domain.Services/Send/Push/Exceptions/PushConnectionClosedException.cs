namespace Domain.Services.Send.Push.Exceptions;

internal sealed class PushConnectionClosedException : Exception
{
    internal PushConnectionClosedException() : base("Other side closed the connection") { }
}