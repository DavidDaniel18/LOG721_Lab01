using System.Linq.Expressions;
using Domain.Common.Monads;

namespace Application.Queries.Interfaces;

public interface ICache<in TKey, TValue> where TKey : notnull
{
    Result<TValue> Get(TKey key);

    List<TValue> Query(Expression<Func<TValue, bool>> filter);
}