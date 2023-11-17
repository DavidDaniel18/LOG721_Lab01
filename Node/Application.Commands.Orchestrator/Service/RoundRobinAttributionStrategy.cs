﻿using Application.Commands.Orchestrator.Interfaces;
using Application.Common.Interfaces;
using Domain.Grouping;
using Domain.Publicity;

namespace Application.Commands.Orchestrator.Service;

internal class RoundRobinAttributionStrategy : IAttributionStrategy
{
    private readonly IHostInfo _hostInfo;

    private readonly RoundRobinAlgorithm _algorithm;

    // todo: get from cache...
    private IDictionary<string, string> _groupIdTopicDict = new Dictionary<string, string>();

    public RoundRobinAttributionStrategy(IHostInfo hostInfo)
    {
        _hostInfo = hostInfo;
        _algorithm = new RoundRobinAlgorithm(_hostInfo.ReduceRoutingKeys.Split(',').ToList());
    }

    public string GetTopicFrom(Group group)
    {
        if (_groupIdTopicDict.TryGetValue(group.Id, out var topic))
            return topic;

        topic = _algorithm.GetNextElement();
        _groupIdTopicDict[group.Id] = topic;

        return topic;
    }
}
