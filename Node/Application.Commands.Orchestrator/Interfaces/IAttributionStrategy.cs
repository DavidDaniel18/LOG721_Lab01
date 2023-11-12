using Domain.Publicity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Orchestrator.Interfaces;

internal interface IAttributionStrategy
{
    string GetTopicFrom(Space space);
}
