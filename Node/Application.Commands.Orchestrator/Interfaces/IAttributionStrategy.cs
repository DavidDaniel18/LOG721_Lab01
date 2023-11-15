using Domain.Grouping;
using Domain.Publicity;

namespace Application.Commands.Orchestrator.Interfaces;

internal interface IAttributionStrategy
{
    string GetTopicFrom(Group space);
}
