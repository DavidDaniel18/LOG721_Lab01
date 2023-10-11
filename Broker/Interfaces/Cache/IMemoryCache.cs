namespace Interfaces.Cache
{
    public interface IMemoryCache<T>
    {
        void Invalidate();
        void SetValue(T value);
        T? GetValue();
    }
}
