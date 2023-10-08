namespace Application.Services.InfrastructureInterfaces;

internal interface IRead
{
    Task<byte[]> ReadAsync(CancellationToken cancellationToken = default);
}