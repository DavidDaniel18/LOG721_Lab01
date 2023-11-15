namespace Domain.Services.Sending.Push.Exceptions;

internal sealed class PushInternalErrorException : Exception
{
    internal PushInternalErrorException() : base("Internal error, error on our side. Closing the connection") { }
}