using System.Collections.Concurrent;
using Domain.Common.Monads;
using Domain.Common.Seedwork.Abstract;

namespace Domain.Caching.ValueObjects;

public abstract class Operation<TAggregate> where TAggregate : Aggregate<TAggregate>
{
    public abstract Result Apply(ConcurrentDictionary<string, TAggregate> dictionary);

    public abstract Result Undo(ConcurrentDictionary<string, TAggregate> dictionary);
}