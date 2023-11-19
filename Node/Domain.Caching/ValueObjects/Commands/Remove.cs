using System.Collections.Concurrent;
using Domain.Common.Monads;
using Domain.Common.Seedwork.Abstract;

namespace Domain.Caching.ValueObjects.Commands;

internal sealed class Remove<TAggregate> : Operation<TAggregate> where TAggregate : Aggregate<TAggregate>
{
    private readonly TAggregate _aggregate;

    public Remove(TAggregate aggregate)
    {
        _aggregate = aggregate;
    }

    public override Result Apply(ConcurrentDictionary<string, TAggregate> dictionary)
    {
        return dictionary.TryRemove(_aggregate.Id, out _)
            ? Result.Success()
            : Result.Failure($"Aggregate {typeof(TAggregate).Name} with id {_aggregate.Id} not found.");
    }

    public override Result Undo(ConcurrentDictionary<string, TAggregate> dictionary)
    {
        dictionary.AddOrUpdate(_aggregate.Id, _ => _aggregate, (_, _) => _aggregate);

        return Result.Success();
    }
}