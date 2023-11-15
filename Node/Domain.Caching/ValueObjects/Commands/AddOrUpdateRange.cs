using System.Collections.Concurrent;
using Domain.Common.Monads;
using Domain.Common.Seedwork.Abstract;

namespace Domain.Caching.ValueObjects.Commands;

internal sealed class AddOrUpdateRange<TAggregate> : Operation<TAggregate> where TAggregate : Aggregate<TAggregate>
{
    private readonly IEnumerable<TAggregate> _aggregates;

    public AddOrUpdateRange(IEnumerable<TAggregate> aggregates)
    {
        _aggregates = aggregates;
    }

    public override Result Apply(ConcurrentDictionary<string, TAggregate> dictionary)
    {
        foreach (var aggregate in _aggregates)
        {
            dictionary.AddOrUpdate(aggregate.Id, aggregate, (_, _) => aggregate);
        }

        return Result.Success();
    }

    public override Result Undo(ConcurrentDictionary<string, TAggregate> dictionary)
    {
        return Result.Foreach(_aggregates, aggregate =>
        {
            dictionary.TryRemove(aggregate.Id, out _);

            return Result.Success();
        });
    }
}