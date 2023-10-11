using Interfaces.Cache;

namespace Entities.Cache
{
    public class InMemoryCache<T> : IMemoryCache<T>
    {
        private T? _value;

        public InMemoryCache(T value) 
        {
            _value = value;
        }

        public T? GetValue()
        {
            return _value;
        }

        public void Invalidate()
        {
            _value = default;
        }

        public void SetValue(T value)
        {
            _value ??= value;
        }
    }
}
