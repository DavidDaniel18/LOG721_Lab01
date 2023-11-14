using Domain.Publicity;

namespace Application.Commands.Orchestrator.Interfaces;

internal interface IAttributionStrategy
{
    string GetTopicFrom(Space space);
}
