using System.Linq.Expressions;
using Domain.Common.Monads;
using Domain.Common.Seedwork.Abstract;

namespace Application.Queries.Interfaces;

public interface ICache<TValue> where TValue : Aggregate<TValue>
{
    Result<TValue> Get(string key);

    List<TValue> Query(Expression<Func<TValue, bool>> filter);
}