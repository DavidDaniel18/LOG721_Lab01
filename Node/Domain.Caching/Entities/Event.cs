using Domain.Caching.ValueObjects;
using Domain.Common.Seedwork.Abstract;

namespace Domain.Caching.Entities;

internal sealed class Event<TAggregate> : Aggregate<Event<TAggregate>>
    where TAggregate : Aggregate<TAggregate>
{
    public Operation<TAggregate> Operation { get; }

    public Event(string id, Operation<TAggregate> operation) : base(id)
    {
        Operation = operation;
    }
}