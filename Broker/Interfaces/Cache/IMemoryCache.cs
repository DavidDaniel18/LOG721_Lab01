using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Cache
{
    public interface IMemoryCache<T>
    {
        void Invalidate();
        void SetValue(T value);
        T? GetValue();
    }
}
