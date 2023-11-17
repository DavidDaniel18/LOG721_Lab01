using System.Linq.Expressions;

namespace Application.Queries.Interfaces;

public interface ICache<in TKey, TValue> where TKey : notnull
{
}