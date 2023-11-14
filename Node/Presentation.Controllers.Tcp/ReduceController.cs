using SmallTransit.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Presentation.Controllers.Tcp;

public class ReduceController : IConsumer<Group>
{
    public Task Consume(Group contract)
    {
        throw new NotImplementedException();
    }
}
