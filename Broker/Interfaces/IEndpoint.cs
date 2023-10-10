using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Domain;

namespace Interfaces
{
    public interface IEndpoint
    {
        void Publish(IPublication publication);
    }
}
