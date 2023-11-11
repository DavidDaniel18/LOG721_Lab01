using System.Collections.Concurrent;
using System.Linq.Expressions;
using Application.Queries.Interfaces;
using Domain.Common.Monads;

namespace Infrastructure.Cache;

internal sealed class Cache<TKey, TValue> : ICache<TKey, TValue> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, TValue> _dictionary = new();

    public List<TValue> Query(Expression<Func<TValue, bool>> filter)
    {
        var func = filter.Compile();

        var filteredItems = _dictionary
            .Where(kv => func(kv.Value))
            .Select(kv => kv.Value)
            .ToList();

        return filteredItems;
    }

    public void AddOrUpdate(TKey key, TValue value)
    {
        _dictionary.AddOrUpdate(key, value, (_, _) => value);
    }

    public void Remove(TKey key)
    {
        _dictionary.TryRemove(key, out _);
    }

    public Result<TValue> Get(TKey key)
    {
        if (_dictionary.TryGetValue(key, out var value))
            return Result.Success(value);

        return Result.Failure<TValue>($"Key {key} not found in cache");
    }
}