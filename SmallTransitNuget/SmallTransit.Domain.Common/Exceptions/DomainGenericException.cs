namespace SmallTransit.Domain.Common.Exceptions;

public sealed class DomainGenericException : Exception
{
    public DomainGenericException() {}

    public DomainGenericException(string message) : base(message) {}
}