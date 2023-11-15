using System.Linq.Expressions;
using Application.Queries.Interfaces;
using Domain.Caching;
using Domain.Caching.ValueObjects;
using Domain.Common.Monads;
using Domain.Common.Seedwork.Abstract;

namespace Infrastructure.Cache;

public sealed class DistributedCache<TValue> : ICache<TValue> where TValue : Aggregate<TValue>
{
    private static readonly Cache<TValue> Cache = new();

    public Result<TValue> Get(string key)
    {
        return Cache.Get(key);
    }

    public List<TValue> Query(Expression<Func<TValue, bool>> filter)
    {
        return Cache.Query(filter);
    }

    public async Task Apply(Operation<TValue> operation)
    {
        await Cache.Apply(operation);
    }
}