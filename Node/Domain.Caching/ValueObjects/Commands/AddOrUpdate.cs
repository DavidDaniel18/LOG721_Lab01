using System.Collections.Concurrent;
using Domain.Common.Monads;
using Domain.Common.Seedwork.Abstract;

namespace Domain.Caching.ValueObjects.Commands;

internal sealed class AddOrUpdate<TAggregate> : Operation<TAggregate> where TAggregate : Aggregate<TAggregate>
{
    private readonly TAggregate _aggregate;

    public AddOrUpdate(TAggregate aggregate)
    {
        _aggregate = aggregate;
    }

    public override Result Apply(ConcurrentDictionary<string, TAggregate> dictionary)
    {
        dictionary.AddOrUpdate(_aggregate.Id, _ => _aggregate, (_, _) =>  _aggregate);

        return Result.Success();
    }

    public override Result Undo(ConcurrentDictionary<string, TAggregate> dictionary)
    {
        return dictionary.TryRemove(_aggregate.Id, out _) ?
            Result.Success() :
            Result.Failure("Failed to remove the aggregate from the dictionary.");
    }
}