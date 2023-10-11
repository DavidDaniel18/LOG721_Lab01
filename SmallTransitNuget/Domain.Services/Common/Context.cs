namespace Domain.Services.Common;

internal abstract class Context
{
    internal Guid Id { get; } = Guid.NewGuid();
}