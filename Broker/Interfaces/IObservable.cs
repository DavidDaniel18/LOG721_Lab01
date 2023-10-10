using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IObservable
    {
        List<IObserver> Observers { get; }
        void Notify();
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
    }
}
