﻿using Application.Commands.Orchestrator.Interfaces;
using Application.Common.Interfaces;
using Domain.Grouping;

namespace Application.Commands.Orchestrator.Service;

public class GroupAttributionService : IGroupAttributionService
{
    private IAttributionStrategy _attributionStrategy;

    internal GroupAttributionService(IHostInfo hostInfo)
    {
        _attributionStrategy = new RoundRobinAttributionStrategy(hostInfo);
    }

    public string GetAttributedKeyFromGroup(Group group)
    {
        return _attributionStrategy.GetTopicFrom(group);
    }
}
