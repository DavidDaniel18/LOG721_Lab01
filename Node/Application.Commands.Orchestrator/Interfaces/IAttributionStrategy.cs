using Domain.Grouping;

namespace Application.Commands.Orchestrator.Interfaces;

internal interface IAttributionStrategy
{
    string GetTopicFrom(Group group);
}
