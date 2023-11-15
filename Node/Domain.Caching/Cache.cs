using Domain.Common.Monads;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using Domain.Caching.Entities;
using Domain.Common.Seedwork.Abstract;
using Domain.Caching.ValueObjects;

namespace Domain.Caching;

public sealed class Cache<TValue> where TValue : Aggregate<TValue>
{
    private readonly ConcurrentDictionary<string, TValue> _dictionary = new();

    private ImmutableList<Event<TValue>> _events = ImmutableList<Event<TValue>>.Empty;

    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private int _checkpoint = 0;

    public List<TValue> Query(Expression<Func<TValue, bool>> filter)
    {
        var func = filter.Compile();

        var filteredItems = _dictionary
            .Where(kv => func(kv.Value))
            .Select(kv => kv.Value)
            .ToList();

        return filteredItems;
    }

    public async Task<Result<string>> Apply(Operation<TValue> operation)
    {
        return await operation.Apply(_dictionary)
            .BindAsync(async () =>
            {
                var @event = new Event<TValue>(Guid.NewGuid().ToString(), operation);

                try
                {
                    await _semaphore.WaitAsync();

                    _events = _events.Add(@event);

                    Interlocked.Increment(ref _checkpoint);

                    return Result.Success(@event.Id);
                }
                catch
                {
                    return Result.Failure<string>("Error while applying operation");
                }
                finally
                {
                    _semaphore.Release();
                }
            });
    }

    public void SaveChangesAsync()
    {
        Interlocked.Exchange(ref _checkpoint, 0);
    }

    public async Task UndoChangesAsync()
    {
        await _semaphore.WaitAsync();

        Interlocked.Exchange(ref _checkpoint, 0);

        var events = _events
            .ToArray()[-_checkpoint..]
            .Reverse()
            .ToArray();

        foreach (var @event in events)
        {
            @event.Operation.Undo(_dictionary);
        }

        _events = _events.RemoveRange(_events.Count - _checkpoint, _checkpoint);

        _semaphore.Release();
    }

    public Result<TValue> Get(string key)
    {
        return _dictionary.TryGetValue(key, out var value) ? 
            Result.Success(value) : 
            Result.Failure<TValue>($"Key {key} not found in cache");
    }
}