using Application.Commands.Orchestrator.Interfaces;
using Application.Common.Interfaces;
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

    public string GetTopicFrom(Space space)
    {
        if (_groupIdTopicDict.TryGetValue(space.GroupId ?? "", out var topic))
            return topic;

        return _algorithm.GetNextElement();
    }
}
