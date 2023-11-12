using Application.Commands.Orchestrator.Interfaces;
using Application.Common.Interfaces;
using Domain.Publicity;

namespace Application.Commands.Orchestrator.Service;

internal class RoundRobinAttributionStrategy : IAttributionStrategy
{
    private readonly IHostInfo _hostInfo;

    private readonly List<string> _publishTopics;

    private int _roundRobinIndex = 0;

    private readonly int _topicsSize;

    // todo: get from cache...
    private IDictionary<string, string> _groupIdTopicDict = new Dictionary<string, string>();

    internal RoundRobinAttributionStrategy(IHostInfo hostInfo)
    {
        _hostInfo = hostInfo;
        _publishTopics = _hostInfo.PublishTopics.Split(',').ToList();
        _topicsSize = _publishTopics.Count();
    }

    private void IncrementRoundRobinIndex() => _roundRobinIndex = (_roundRobinIndex + 1) % _topicsSize;

    public string GetTopicFrom(Space space)
    {
        string? topic;
        if (_groupIdTopicDict.TryGetValue(space.GroupId ?? "", out topic))
            return topic;

        topic = _publishTopics.ElementAt(_roundRobinIndex);
        IncrementRoundRobinIndex();

        return topic;
    }
}
