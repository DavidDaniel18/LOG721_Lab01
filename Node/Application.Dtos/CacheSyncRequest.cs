using Domain.Common.Seedwork.Abstract;

namespace Application.Dtos;

public sealed class CacheSyncRequest<T> where T : Aggregate<T>
{
    public Guid Id { get; set; }

    public CacheSyncRequest(Guid id, T value)
    {
        Id = id;
        Value = value;
    }
}