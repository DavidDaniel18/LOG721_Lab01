using System.Collections.Concurrent;

namespace Application.Common.Interfaces;

public interface ISingletonCache<T>
{
    ConcurrentDictionary<string, T> Value { get; }
}
