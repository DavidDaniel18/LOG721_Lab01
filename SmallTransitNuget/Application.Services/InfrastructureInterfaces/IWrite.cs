namespace Application.Services.InfrastructureInterfaces;

internal interface IWrite
{
    Task WriteAsync(byte[] buffer, CancellationToken cancellationToken = default);
}