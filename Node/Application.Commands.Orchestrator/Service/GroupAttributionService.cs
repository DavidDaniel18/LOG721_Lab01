using Application.Commands.Orchestrator.Interfaces;
using Application.Common.Interfaces;
using Domain.Publicity;

namespace Application.Commands.Orchestrator.Service;

public class GroupAttributionService : IGroupAttributionService
{
    private IAttributionStrategy _attributionStrategy;

    internal GroupAttributionService(IHostInfo hostInfo)
    {
        _attributionStrategy = new RoundRobinAttributionStrategy(hostInfo);
    }

    public string GetAttributedKeyFromSpace(Space space)
    {
        return _attributionStrategy.GetTopicFrom(space);
    }
}
