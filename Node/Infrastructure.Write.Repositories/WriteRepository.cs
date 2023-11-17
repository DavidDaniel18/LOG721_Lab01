using Domain.Common.Monads;
using Domain.Common.Seedwork.Abstract;
using Infrastructure.Cache;

namespace Infrastructure.Write.Repositories;

public abstract class WriteRepository<TAggregate> where TAggregate : Aggregate<TAggregate>
{
    //protected readonly DistributedCache<TAggregate> DistributedCache = new();
    //
    //public virtual Result<TAggregate> GetAsync(string id)
    //{
    //    return DistributedCache.Get(id);
    //}
    //
    //public virtual Task<Result<TAggregate>> AddAsync(TAggregate aggregate)
    //{
    //    //return DistributedCache.Apply(new OperatingSystem());
    //    Task.CompletedTask;
    //}
}
