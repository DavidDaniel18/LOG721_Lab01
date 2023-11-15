using Application.Common.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Cache;

public class SingletonCache<T> : ISingletonCache<T>
{
    public ConcurrentDictionary<string, T> _value = new ConcurrentDictionary<string, T>();
    ConcurrentDictionary<string, T> ISingletonCache<T>.Value => _value;
}
